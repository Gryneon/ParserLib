I need a windows forms C# application (Just the .cs source files though).

This application must run on windows 11 24H2 natively without special packages installed. (No preview .net versions)

The application must generate specific labels and I will be providing a template. This template will be stored in .txt files in a folder that is in the same directory as the program. The folder is named "templates".

Any .txt file in this folder will be an available template selectable from the program.
The templates display name will be the filename minus .txt.
Use 2 Form objects, one for template options, one additional popup for printing.
The primary form, will load the template, look for any and all instances of `<<variable_name>>` (without the outer quotes)
Each variable must be assignable to a value, shown in an editable DataGrid Control.
There must be a print button that opens up the print form that only enables after a template has been selected.
The application must be intuitive.
The print form must have a `Print` button, and a `Cancel` button, as well as a filled out text preview of the label.
The `print` button must send a file named `label.pr1` via FTP using `sato` for the user, and `pass` for the password to an IP, `10.68.89.233`. Use the standard FTP port.
The IP, user, and password must be overridable via text boxes on the form, but default to the values stated above.
