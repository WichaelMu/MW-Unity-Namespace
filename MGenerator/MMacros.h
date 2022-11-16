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
