# LiveSplit.BlackScreenDetector
LiveSplit component to automatically detect and remove black screens.

This is adapted from my standalone detection tool https://github.com/thomasneff/CrashNSaneTrilogyLoadDetector
and from https://github.com/Maschell/LiveSplit.PokemonRedBlue for the base component code.

# Other Image/Video-Based AutoSplitters and/or Load Removers
This component *only removes and detects black screens, nothing else!*

If you want to

 * Auto-split based on a folder of split images: https://github.com/Toufool/Auto-Split by https://github.com/Toufool
 * Auto-split and remove loads based on scriptable events from a video feed: https://github.com/ROMaster2/LiveSplit.VideoAutoSplit by https://github.com/ROMaster2
 * Auto-split and remove loads from only the Crash N. Sane Trilogy: https://github.com/thomasneff/LiveSplit.CrashNSTLoadRemoval by https://github.com/thomasneff/.


# Special Thanks
Special thanks go to McCrodi from the Crash Speedrunning Discord, who helped me by providing 1080p/720p captured data and general feedback regarding the functionality.

# How does it work?
The method works by taking a small "screenshot" (currently 600x100) from your selected capture at the top-center of the capture region.
There, it detects the maximum brightness of all pixels and detects it as a black screen if none of the pixels are brighter than the black level. 
This is a very conservative metric, and should only detect black screens if they are fully black. If there is need for some tolerance, the metric needs to be adapted. 

# Settings
The LiveSplit.BlackScreenDetector.dll goes into your "Components" folder in your LiveSplit folder.

Add this to LiveSplit by going into your Layout Editor -> Add -> Control -> BlackScreenDetector.

You can specify to capture either the full primary Display (default) or an open window. This window has to be open (not minimized) but does not have to be in the foreground.

Make sure that you *capture the full game region* as accurately as possible!

This might not work for windows with DirectX/OpenGL surfaces, nothing I can do about that. (Use Display capture for those cases, sorry, although even that might not work in some cases). In those cases, you will probably get a black image in the capture preview in the component settings.

