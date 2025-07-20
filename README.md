- [Syntax](#syntax)
- [Screens and Windows](#Screens-and-windows)
- [Running Commands](#running-commands)
- [Special Functions](#special-functions-and-addresses)
- [Colors Table](#colors-table)


# Brainfuck++
> ***I dont know why i did this and i would rather not know***
---
## Syntax
Im sure most of you know about the 8 base characters used in Brainfuck but either way, I will go over them 
- ### ">" And "<"
    Simple, Brainfuck has 30,000 memory cells and 80,000 in my case and the > and < are used to make the memory pointer move either left or right 

- ### "+" And "-"
  Self explanatory, Adds or subtracts the number present in the current cell the pointer is at (Each cell is a byte (0-255))

- ### "," And "."
  "*,*" Is used to read user input and put its ***ASCII*** representation in the current cell, As you might have guessed the "*.*" Is the exact opposite.

- ### "[" And "]"
  ***Those ones are a bit complicated so i might not explain them well***
    Either way i will take a shot at it, The "[" works by checking if the current cell is 0 or not, If it is, Then jump to the corresponding "]" And the closing bracket does the exact opposite, Creating conditional loops
---
## That wraps it up for the base instructions, However i added 3 more


- ### The "{N}"
  Jumps to a certain memory address,
  `{5315}` Jumps to address 5315
- ### The "^N\^"
  Usage same as before but instead sets the current value directly
- ### The "(N)"
  More obscure and i doubt you will be using it but it basically makes the instruction pointer jump, So you have to keep track of the count of instructions if you want to practically use this
---
## Now for the exciting part,
- ### Screens and windows
  Now you can make a window and write to each pixel manually, ~~How exciting~~ 
  1. #### Setup: 
  2. Set address 250 to 1
  3. Set 251 to the X size in pixels and 252 for the Y
  4. 253+ X*Y is now video memory and 0-15 represents each color (More on that later)
  5. #### Pixel data:
  6. You manually write to each pixel I.e `253-65,278` at max (About 65,025)
  7. #### Giving the display confirmation
  8. Just set address 79998 to 1 (Then maybe reset it so you dont nuke your memory on accident (More on that later))
  9. And now you have a window, The pixel data updates in real time
- ### Running commands
  Because running commands can be quite dangerous, You will need to place the second argument as AC (Allow commands)
  1. ##### Setup:
  2. Address `192-248` are the command buffer, Only ***ASCII*** characters in decimal form are allowed
  3. ##### Running the command:
  4. Set address 249 to 1, It will automatically be reset by the interpreter after
  5. ##### Obtaining the output
  6. The output of the command will be from 65279 to 65343 (64 bytes)
- ### Special functions and addresses
  - #### Extra confirmation
    Some functions will require extra confirmation either because they are very heavy in resources, Or to act as an extra "Are you sure?"
  - #### Delays
    To delay program execution all you need to do is put in address 79997 the number of milliseconds you want to wait for (Will be automatically reset after by the interpreter)
  - #### Clearing memory
    I have added a ... Quite special function that will reset or clear the entire memory tape, You run it by setting Ad. 79998 to 1 along with 79996 the entire tape will be reset except for 79999 which is SUPPOSED to be accessed by the interpreter only but im sure you can figure out what its for, And maybe even have some fun with it
  - #### End program execution
    Now sometimes if the instructions end, But some other things are running or another window is open or anything really, I have implemented a "Shutdown sequence" so it ensures the program ends leaving nothing behind
  ##### 1. Set memory address 79998 to 1
  ##### 2. Set memory address 79996 to 1
  ##### 3. Set memory address 79997 to 1
  ##### 4. Set memory address 79995 and 79998 to 1 and now the program safely exits
- ### Colors table
  | Color    | Code 0-15  |
  |--------|--------|
  | Black      | 0  |
  | Dark Gray  | 1  |
  | Gray       | 2  |
  | Light Gray | 3  |
  | White      | 4  |
  | Red        | 5  |
  | Green      | 6  |
  | Blue       | 7  |
  | Yellow     | 8  |
  | Orange     | 9  |
  | Purple     | 10 |
  | Maroon     | 11 |
  | Lime       | 12 |
  | Sky Blue   | 13 |
  | Brown      | 14 |
  | Pink       | 15 |





  Note: I forgot to add this while writing but there is a debug mode basically by making the 3rd argument when running the program be Debug
  (1st argument is path of the bf++ script, 2nd is AC (Allow commands) and 3rd is debug)
