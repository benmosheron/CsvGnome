# CsvGnome
---
## A Simple but Clever CSV File Generator for Windows 10.

Specify fields and values with `:`.

* `fieldName:ValueName` creates a field (column) with name "fieldName" and all values "ValueName"

Change the number of rows to generate by entering a number E.g. `12` to write twelve rows of data, this doesn't include the columns row.

* `help` displays help.
* `exit` quits.
* `write` writes the CSV file. The file will be written to CsvGnome's Output directory.
* `run` writes the CSV and quits.
* `delete fieldName` deletes the field with name "fieldName".
* `clear` clears all fields.

Special **components** can be added to values (`help special` displays a list of available components):
* `[++]` counts up from zero.
* `[3++]` counts up from three.
* `[19++2]` counts up from nineteen in steps of two.
* `[date]` the date and time, at the time the file is generated.
* `[1=>5]` cycles the values from one to five.
* `[0=>50, 10]` cycles the values from zero to fifty in steps of ten.
* `[a=>d]` cycles the letters a,b,c, and d.
* `[C=>a]` cycles C,B,A,c,b,a,C...

Specify arrays of values manually with `[spread]` and `[cycle]`

* `[spread]{a,b,c}` spreads the values from inside the curly braces (e.g. `a,a,a,b,b,b,c,c`)
* `[cycle]{a,b,c}` cycles the values from inside the curly braces (e.g. for 8 rows: `a,b,c,a,b,c,a,b`)
  * Use `full on` or `full off` to display full array contents, or a summary of array contents in the console.

### Combinatorials
Certain components can be tagged with a group ID (and, optionally, a rank) in order to cycle through every possible *combination* of values in that group.


Tag a component by adding `#<groupId>` before the last square bracket of the component (for example `[1=>5 #meow]`). 


A component with a lower rank will cycle through all of its possible values before the component with the next rank up will move to its next element.
Specify a rank by adding `/<rank>` (where the rank is an integer) after the ID, but still inside the square brackets. If you don't specify a rank, they will be allocated in the order they are read.


Each component can only belong to one combinatorial group, but you can have as many different groups as you like, and the components can be spread over different fields - or all contained in the same field.


The following components can be tagged with group IDs:
* MinMax `[1=>4, 3 #example/0]`.
* Alphabet `[z=>a #example/1]`.
* Cycling arrays `[cycle #example/2]{boop,beep}`.
* Incrementing `[++ #example/3]` (these will continue to increase forever, so make sure they are the highest rank in their groups - no components with higher ranks will ever go past their first elements!).

#### Combinatorial Example

Let's demonstrate with a few CsvGnome fields:
```
20
Test Location:[A=>C #gridRef/1][0=>2 #gridRef/0]
Test Subjects:[cycle #gridRef/2]{bee,wasp}
write
```

This will get us the following output:

*CSVs have been converted to tables to display nicely.*

|Test Location|Test Subjects|
|---|---|
|A0|bee|
|A1|bee|
|A2|bee|
|B0|bee|
|B1|bee|
|B2|bee|
|C0|bee|
|C1|bee|
|C2|bee|
|A0|wasp|
|A1|wasp|
|A2|wasp|
|B0|wasp|
|B1|wasp|
|B2|wasp|
|C0|wasp|
|C1|wasp|
|C2|wasp|
|A0|bee|
|A1|bee|


### Example Input
```
// Enter a number to set the number of rows to generate.
12
// Create fields.
// Plain text
Location:London
// Date and increment
UniqueId:[date]_ID[++]
// Some combinatorial MinMax and Incrementing components
x:[1=>2, 1 #pos]
y:[1=>5, 2 #pos]
z:[++ #pos]
write
```

#### Example Output

*CSVs have been converted to tables to display nicely.*

|Location|UniqueId|x|y|z|
|--------|--------|---|---|---|
|London|2016-05-15T08:54:46_ID00|1|1|0|
|London|2016-05-15T08:54:46_ID01|1|1|1|
|London|2016-05-15T08:54:46_ID02|1|3|0|
|London|2016-05-15T08:54:46_ID03|1|3|1|
|London|2016-05-15T08:54:46_ID04|1|5|0|
|London|2016-05-15T08:54:46_ID05|1|5|1|
|London|2016-05-15T08:54:46_ID06|2|1|0|
|London|2016-05-15T08:54:46_ID07|2|1|1|
|London|2016-05-15T08:54:46_ID08|2|3|0|
|London|2016-05-15T08:54:46_ID09|2|3|1|
|London|2016-05-15T08:54:46_ID10|2|5|0|
|London|2016-05-15T08:54:46_ID11|2|5|1|


### GnomeFiles

Save and load configurations of fields to GnomeFiles with `save` and `load` commands. Gnomefiles can be easily edited in a text editor. The GnomeFile "default.gnome" is automatically loaded when CsvGnome runs, you can edit this file to contain a common set of fields.

* `gnomefiles` displays the available gnomefiles, along with their locations.
* `save example` saves the current fields to "example.gnome".
* `load example` loads the previously saved fields from "example.gnome".

### Scripting

Add custom functions to `Scripts\functions.lua` to allow them to be used in components of fields. Lua functions must take a single input parameter "i", which will be passed the zero-based row number.

Functions must have the following method signature and format:
```
function <function name>(i)
  <function body>
  return <any value>
end
```

These functions can then be called by CsvGnome components like so: `[lua <function name>]`.

#### Scripting Example
Given the following contents of `Scripts\functions.lua`:

```
function uniform(i)
  return math.random()
end

function uniform2digits(i)
  return tostring(uniform()):sub(3,4)
end

function square(i)
  return i*i
end 
```

And the following CsvGnome input:
```
Scripting Test:omg! [lua uniform2digits]... OMG! [lua square]
```

We will get the following output (your random numbers *may* vary):

*CSVs have been converted to tables to display nicely.*

|Scripting Test|
|--------------|
|omg! 49... OMG! 0|
|omg! 28... OMG! 1|
|omg! 43... OMG! 4|
|omg! 35... OMG! 9|
|omg! 07... OMG! 16|

### Info

* CsvGnome is the creation of Ben Sheron. I welcome correspondence at benmosheron@gmail.com
* Source code is available at https://github.com/benmosheron/CsvGnome
* Any ideas or feedback? File an issue at https://github.com/benmosheron/CsvGnome/issues/new
* Lua scripting capability is provided by [NLua](http://nlua.org/)