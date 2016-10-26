## 1.5.0
* Breaking change: script functions are now provided an IScriptArgs "args" object. instead of a single integer.
  * Currently args gives access to two properties, i (the row index) and N (the number of rows).
* You can now optionally provide a [.NET date format string](https://msdn.microsoft.com/en-us/library/8kb3ddd4) to [date] components.
  * e.g. `[date "HH:mm dddd dd MMMM"]` writes `19:09 Tuesday 18 October`.
* View a preview of the data with `preview`.
* Optionally enable padding of fields with spaces using `pad on` and `pad off`.
* New component `[N]` substitutes the total number of rows. This one was easy!
* You can now use `[i]` as a component, which does the exact same thing as `[++]`, by providing the zero-based row index.
* Bug fix: Inserting a combinatorial component at a rank that already exists will now bump up every higher rank to make room (rather than crashing).
* Bug fix: You can't set N to be negative any more...
* Under the hood:
  * Split Console app into CsvGnomeStandAlone, with core functionality remaining in CsvGnome.
  * CsvGnomeScript has been separated from CsvGnome via CsvGnomeScriptApi, to help keep the two decoupled for the future.
  * Separated Lua scripting capability into separate libraries to isolate the dependency on NLua.

## 1.4.0
* Overhauled combinatorial fields. The following components can now be given an ID to combinatorially cycle through fields in a group:
** `[++ #example/3]`
** `[1=>3 #example/0]`
** `[a=>c #example/1]`
** `[cycle #example/2]{cat,dog}`
* You can now specify the rank of a combinatorial component. Lower ranks go through a complete cycle before incrementing the next rank up.
* MinMax components e.g. `[0=>-30, -3]` work with negative numbers. Make sure you get the signs right, or you'll get an error.
* New Alphabet component `[a=>z]` will cycle through alphabet characters, between the input start and end.
* Cleared up the code base in general.
* New unit tests across the board.
* Added lua scripting functionality. Lua functions which take a single parameter - the row number - can be added to Scripts\functions.lua. These functions will be called by `[lua <function name>]` components.

## 1.3.0
* Fixed bug where array components would not work for large arrays.
* Removed use of dark blue colour for text, it was too hard to read.
* Command `save` without a filename now saves the current loaded gnomefile (default by default).
* Console window title displays the currently loaded gnomefile.
* Added colour to `gnomefiles` list.