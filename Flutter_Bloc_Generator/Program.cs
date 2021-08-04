using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Flutter_Bloc_Generator
{
    class Program
    {
        static string importEquatableString = "import 'package:equatable/equatable.dart';";
        static string importBlocString = "import 'package:bloc/bloc.dart';";
        static string partEventFileString = "part '{bloc}_event.dart'";
        static string partStateFileString = "part '{bloc}_state.dart'";
        static string partBlocFileString = "part of '{bloc}_bloc.dart'";
        static void Main(string[] args)
        {
            List<Bloc> blocClasses = new List<Bloc>() 
            { 
                new Bloc() 
                {
                    BlocName ="TodoBloc",
                    EventName="TodoEvent",
                    StateName="TodoState",
                    FileName="todo",
                    EventsNames = new List<string>() { "SendMobileVerification", "MobileVerificationCodeSent" },
                    StatesName= new List<string>() { "InitialState", "Loaded" } 
                } 
            };

            foreach (var item in blocClasses)
            {
                var baseEventClass = GenerateAbstractClassString(item.EventName, item.FileName);
                var baseStateClass = GenerateAbstractClassString(item.StateName, item.FileName);
                var eventClassString = String.Join($"{Environment.NewLine}{Environment.NewLine}",item.EventsNames.Select(x => GenerateSubClassString(x, item.EventName)).ToList());
                var stateClassString = String.Join($"{Environment.NewLine}{Environment.NewLine}", item.StatesName.Select(x => GenerateSubClassString(x, item.StateName)).ToList());
                var blocClassString = GenerateBlocClassString(item.EventName,item.StateName,item.BlocName,item.StatesName.First(),item.EventsNames,item.FileName);

                if (!Directory.Exists($"{item.FileName}_bloc"))
                {
                    Directory.CreateDirectory($"{item.FileName}_bloc");
                }

                File.WriteAllText($"{item.FileName}_bloc/{item.FileName}_event.dart", $"{baseEventClass}{Environment.NewLine}{eventClassString}");
                File.WriteAllText($"{item.FileName}_bloc/{item.FileName}_state.dart", $"{baseStateClass}{Environment.NewLine}{stateClassString}");
                File.WriteAllText($"{item.FileName}_bloc/{item.FileName}_bloc.dart", blocClassString);


                Console.WriteLine(eventClassString);
                Console.WriteLine(stateClassString);
                Console.WriteLine(blocClassString);
            }


        }


        private static string GenerateAbstractClassString(string className, string bloc)
        {
            var abstractClassDeclare = $"abstract class {className} extends Equatable"+" {";
            var constructor = $" const {className}();";
            var overrideString = " @override";
            var overrideMethod = " List<Object> get props => [];";
            var partOf = partBlocFileString.Replace("{bloc}", bloc);


            return $"{partOf}{Environment.NewLine}{Environment.NewLine}{abstractClassDeclare}{Environment.NewLine}{constructor}{Environment.NewLine}{Environment.NewLine}{overrideString}{Environment.NewLine}{overrideMethod}{Environment.NewLine}" +"}";
        }

        private static string GenerateSubClassString(string className, string baseClass)
        {
            var abstractClassDeclare = $"class {className} extends {baseClass}" + " {";
            var constructor = $" const {className}();";

            return $"{abstractClassDeclare}{Environment.NewLine}{constructor}{Environment.NewLine}" + "}";
        }

        private static string GenerateBlocClassString(string eventClassName, string stateClassName, string blocClassName, string initialState, List<string> events, string bloc)
        {
            var classDeclare = $"class {blocClassName} extends Bloc<{eventClassName}, {stateClassName}>"+" {";
            var constructor = $" {blocClassName}() : "+$"super({initialState}());";
            var overrideString = " @override";
            var mapEventToStateString = $" Stream<{stateClassName}> mapEventToState({eventClassName} event) async*" + " { eventMapping"+" }";
            string eventMapping = String.Empty;
            string eventMappingFunctions = String.Empty;
            foreach (var item in events)
            {
                eventMapping += $"{Environment.NewLine}  if (event is {item})" + " {" + Environment.NewLine + $"    yield* _map{item}ToState(event);"+ Environment.NewLine+ "  }" + Environment.NewLine;
                eventMappingFunctions += $" Stream<{stateClassName}> _map{item}EventToState({item} event) async*"+" {}" + Environment.NewLine + Environment.NewLine;
            }
            mapEventToStateString = mapEventToStateString.Replace("eventMapping", eventMapping);
            var partOfEventString = partEventFileString.Replace("{bloc}", bloc);
            var partOfStateString = partStateFileString.Replace("{bloc}", bloc);

            return $"{importEquatableString}{Environment.NewLine}{importBlocString}{Environment.NewLine}{partOfEventString}{Environment.NewLine}{partOfStateString}{Environment.NewLine}{Environment.NewLine}{classDeclare}{Environment.NewLine}{constructor}{Environment.NewLine}{Environment.NewLine}{overrideString}{Environment.NewLine}{mapEventToStateString}{Environment.NewLine}{Environment.NewLine}{eventMappingFunctions}"+"}";

        }
        
    }
}
