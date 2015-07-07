This is my personal project which I use when some company asks me to provide my code sample along with CV.

1. Problem Statement

You need to write a program for tranforming equations into canonical form: unites summands with same variables and equates sums to zero.
An equation can be of any order, contain  any number of variables, could be written with paranthesis.
 
Input format:
P1 + P2 + ... = ... + PN
where P1..PN are summands of form:
ax^k
где a - floating point number;
k - integer;
x - variable (a summand can have multiple variables).
 
For example:
x^2 + 3.5xy + y = y^2 - xy + y
Should be traformed to:
x^2 - y^2 + 4.5xy = 0

The program should support 2 work modes: interactive mode and file mode. 

2. Implementation remarks:

- File and interactive mode aren't implemented yet.

- Correctness of code are proved by extensive unit-tests coverage.

- Parsers are not ideal: so I suppose incorrect work is possible for some quirk corner-cases.

- There is no explanation of how to treat "2xyz^3"in the problem statement. I treat it as 2*(x*y*z)^3 and not as "2*x*y*(z^3)".

- The ptogram just cuts off paranthesis. Addition operation is commutative and associative, so paranthesises affect nothing.
