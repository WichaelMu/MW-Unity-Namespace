# MW.IO.I
Helper class for Mouse and Keyboard Input.

## Mouse Input
Check if a mouse button has been pressed, held, or released.
```cs
EButton MB = EButton.LeftMouse;

bool bClicked = I.Click(MB, false, false); // True if Clicked once, not held or up.
bool bHeld = I.Click(MB, true, false);     // True if Held continuously, not down, or up.
bool bUp = I.Click(MB, false, true);       // True if Released, not down, or up.
bool bAny = I.Click(MB, true, true);       // True if MB was pressed at all.
```

## Keyboard Input
Check if a keystroke has been pressed, held, or released.
```cs
KeyCode KS = KeyCode.M;

bool bPressed = I.Key(KS, false, false);   // True if Pressed once, not held or up.
bool bHeld = I.Key(KS, true, false);       // True if Held continuously, not down, or up.
bool bUp = I.Key(KS, false, true);         // True if Released, not down, or up.
bool bAny = I.Key(KS, false, true);        // True if KS was pressed at all.
```

## Any
Check if anything was pressed.
```
bool bAnythingWasPressed = I.Any();        // True if anything was pressed.
```