This is a standalone portable library for calculating Mandelbrot Sets.  It is not particularly fast (it uses doubles rather than fixed point numbers for example), but does illustrate an interesting algorithm called Contour Crawling.  

The real intent of this code though is to enable generating random interesting Mandelbrot Sets without human input.  

The Contour Crawling code was a port of some C and assembly code my friend Scott Sherman and I wrote 25 years ago.  I checked mand.c in for reference purposes.