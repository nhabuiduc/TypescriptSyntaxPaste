# TypescriptSyntaxPaste

- Visual Studio Extension which converts C# SYNTAX to Typescript SYNTAX, you just need to Copy C# code, and paste into Typescript.ts source code.    

- There is option which will convert class/struct to Typescript interface. (In VS 2015 goto Tool->Option->Typescript Paste-> Convert to interface:true)    

**HOW TO DEBUG/RUN.**    

Need Visual Studio 2015 + install VS 2015 SDK to compile and run this extension.        
Then you need to configure project to allow you debug with VS 2015: go to project Property, Debug tab -> 
- Select Start External Program, and fill your VS2015 path (example: C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe)
- Command line arguments: /rootsuffix Exp

**BRIEF CODE INFORMATION**

Almost all converting classes are in folder Translation, which for each file containing the convert method to convert one kind of
syntax (C#) to Typescript. For example ````ClassDeclarationSyntax```` in Roslyn will be ````ClassDeclarationTranslation```` in this project.

Let say you want convert :      
in C#: ````class A {}````    
typescript: ````class myA{}````      

you just need to navigate to class ClassDeclarationTranslation in project, then change this line:    
````
 return $@"{GetAttributeList()}export class {Syntax.Identifier}{TypeParameterList?.Translate()} {baseTranslation}
           {{
           {Members.Translate()} 
           }}";
 ````    
to:    
````
 return $@"{GetAttributeList()}export class my{Syntax.Identifier}{TypeParameterList?.Translate()} {baseTranslation}
           {{
           {Members.Translate()} 
           }}";
````    

For more information about Syntax Tree in Roslyn, you can use Syntax Visualizer which is included in VS 2015 (View -> Other Windows-> Syntax Visualizer).
