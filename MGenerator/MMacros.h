#pragma once

// Build.

#define BUILD !_DEBUG
#define PRINT_DEBUG_MSGS 0
#define WITH_VS _MSC_VER >= 1932

// Shorthand.
#define VT(T) std::vector<T>

// For Reader.
#define TYPE "TYPE"
#define PROPERTY "PROPERTY"
#define FIELD "FIELD"
#define MEMBER "MEMBER"

// Debug
/* If we are debugging through the Visual Studio Debugger. */
#define EXEC_FROM_VS 0
/* Write the .html files that are created. */
#define WRITE_CREATION_MESSAGES 0
/* Write the line of .cpp code responsible for writing to the .html documentation file. */
#define WRITE_DEBUG_LINES 0
/* Write the class and MEMBER if it has no decorations. */
#define WRITE_NO_DECORATIONS 1
