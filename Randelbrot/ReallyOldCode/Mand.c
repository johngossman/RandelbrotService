/* MAND.C
	(C) Copyright 1988-1996 by SoftSource.  All rights reserved.

*/
#include "control.h"
#include "stdio.h"
#include <stdlib.h>
#include "math.h"
#include "trans.h"
#include "event.h"
#include "vec.h"
#include "metawndo.h"
#include "event.pro"

#if MANDELBROT

#if ASM8087
getCount(double x,double y,double four,int count)
{
	int statw;
	asm finit
	asm fld QWORD PTR four
	asm fld QWORD PTR y	 /* y */
	asm fld QWORD PTR x   /* x y */
	asm fld st(1)			 /* y x y */
	asm fld st(1)			 /* x y x y */
	asm mov cx,WORD PTR count
start: 
	asm fld st(1)		   /* y x y cx cy */
	asm fmul st,st		/* y^2 x y cx cy*/
	asm fld st(1)		/* x y^2 x y cx cy*/      
	asm fmul st,st	   /* x^2 y^2 x y cx cy*/
	asm fld st	      /* x^2 x^2 y^2 x y cx cy */
	asm fadd st,st(2)	 /* x^2+y^2 x^2 y^2 x y cx cy 4.0 */     
	asm fcomp st(7)      /* x^2 y^2 x y cx cy 4.0 */
	asm fstsw statw
	asm fwait
	asm mov ax,statw
	asm sahf
	asm jnc done
	asm fsub st,st(1)	   		/* x^2-y^2 y^2 x y cx cy */
	asm fadd st,st(4)				/* x^2-y^2+cx y^2 x y cx cy */
	asm fstp st(1) 				/* x^2-y^2+cx x y cx cy */
	asm fld st(2) 					/* y x^2-y^2+cx x y cx cy */
	asm fmul st,st(2)  			/* x*y x^2-y^2+cx x y cx cy */
	asm fadd st,st 				/* 2xy x^2-y^2+cx x y cx cy */
	asm fadd st,st(5) 			/* 2xy+cy x^2-y^2+cx x y cx cy*/
	asm fstp st(3)	 				/* x^2+y^2+cx x 2xy+cy cx cy */
	asm fstp st(1)	 				/* x^2+y^2+cx 2xy+cy cx cy  */
	asm loop start
done:  
	asm finit
	asm fwait
	return(_CX);
}
#else
static int getCount(double cx,double cy,double four,int maxcount)
{
	double x,y;
	double x2,y2;
	int count;

	count = maxcount;
	x = cx;	y = cy;
	x2 = x*x;y2 = y*y;

	while ((x2 + y2 < 4) && (count > 1))
		{
		y = 2*x*y + cy;
		x = x2-y2 + cx;
		x2 = x*x;
		y2 = y*y;
		count--;
		}
	return (count);

}
#endif



static int Trigger;
static double gap;
static double leftx,lowy;
static int BigX,BigY;
static int MaxCount;
static int *ColorChart;

static int getMand(int i,int j)
{
	int count;
	double x,y,t;
	Trigger = 0;
	if ((i < 1) || (i > BigX) || (j < 1) || (j > BigY))
		return 0;
	if ((count = GetPixel(i,j)) == 0)
	{
		Trigger = 1;
		x = i*gap+leftx;
		y = j*gap+lowy;
		count = getCount(x,y,4.0,MaxCount);
		count = ColorChart[count];
		PenColor(count);
		SetPixel(i,j);
	}
	return count;
}

int MaxColors;

static void setColors(int *Chart,int Count,double userbands)
{
	double mult,temp;
	int i,last,cur;
	if (userbands == 0)
		{
		if (MaxColors > 16)
			mult = 95.0; /* To combine fewer bands, make this number bigger */
		else
			mult = 35.0;    /* To combine more bands, make this number smaller */
		}
	else
		mult = userbands;
	for(i = 1; i < Count; i++)
	{
		temp = log((double)(Count - i)) * mult;
		Chart[i] = (int)(temp + 1);
	}
	if (MaxColors >= 8)
		Chart[0] = 8;
	else
	  	Chart[0] = 1;
	Chart[Count] = 1;
	last = Chart[1];cur = 1;
	for(i = 1; i < Count; i++)
	{
		Chart[i] = cur;
		if (last != Chart[i+1])
		{
			cur++;
			last = Chart[i+1];
		}
		if (cur == 8)
			cur++;
		if (cur > MaxColors-1)
			cur = 1;
	}
}

static int inversesave;


static int maxx;

static int crawl(DISPLAYDEV dp,int fx,int fy,int thecount)
{
	register int x,y,xinc,yinc;
	int done = 0;
	xinc = 1;
	maxx = x = fx;y = fy;
	while(!done)
	{
		if (!getevent(dp))
			return -1;
		if (dp->PortEvent.type)
			return -1;
		if (getMand(x+xinc,y) != thecount)
			yinc = xinc;
		else
		{
			yinc = -1*xinc;
			x+= xinc;
			if (x > maxx)
				maxx = x;
			done = ((fx == x) && (fy == y));
		}
		if (getMand(x,y+yinc) != thecount)
			xinc = -1*yinc;
		else
		{
			xinc = yinc;
			y+=yinc;
			done = ((fx == x) && (fy == y));
		}
	}
	return 1;
}


static int getMand2(int i,int j)
{
	if ((i < 1) || (i > BigX) || (j < 1) || (j > BigY))
		return 0;
	return (GetPixel(i,j));
}


static void filLine(int x,int y,int inc)
{
	register int temp;
	int inc2 = inc+inc;
	if (getMand2(x+inc,y) == 0)
	{
		temp = x+inc;
		while(getMand2(temp,y) == 0)
		{
			temp+=inc2;
			if (temp >= maxx) break;
			if (temp < 1) break;
		}
  		MoveTo(x,y);
		if (temp > 2)
		{
			if (temp-inc >= maxx)
				LineTo(maxx,y);
			else
				LineTo(temp-inc,y);
		}
		else
			LineTo(1,y);
	}
}


static int fillin(DISPLAYDEV dp,int fx,int fy,int thecount)
{
	register int x,y,xinc,yinc;
	int done = 0;

	xinc = 1;
	x = fx;y = fy;
	PenColor(thecount);
	while(!done)
	{
		if (!getevent(dp))
			return -1;
		if (dp->PortEvent.type)
			return -1;
		if (getMand2(x+xinc,y) != thecount)
			yinc = xinc;
		else
		{
			yinc = -xinc;
			x+= xinc;
			if (yinc < 0)
				filLine(x,y,1);
			else
				filLine(x,y,-1);
			done = ((fx == x) && (fy == y));
		}
		if (done) break;
		if (getMand2(x,y+yinc) != thecount)
			xinc = -yinc;
		else
		{
			xinc = yinc;
			y+=yinc;
			if (yinc < 0)
				filLine(x,y,1);
			else
				filLine(x,y,-1);
			done = ((fx == x) && (fy == y));
		}
	}
	return 1;
}

int mand(DISPLAYDEV dp,MAND mand)
{
	struct PortInfo *P;
	double res;
	double temp;
	register int i,j,count;
	int lastcount = 0;
	int lasty,times;

	MaxColors = QueryColors();
	P = dp->getportinfo(dp);
	if (P->SizeX < P->SizeY)
		res = P->SizeX;
	else
		res = P->SizeY;
	BigX = P->SizeX;
	BigY = P->SizeY;
	mand->Mand.gap = gap = mand->Mand.side / res;
	if (mand->usercount == 0.0)
		{
		temp = (log(1.0 / mand->Mand.side));
		temp *= temp * 20;
		mand->Mand.count = temp + 200;
		}
	else
		mand->Mand.count = mand->usercount;
	leftx = mand->Mand.leftx;
	lowy = mand->Mand.lowy;
	MaxCount = mand->Mand.count;
	times = 0;
	ColorChart = (int *)malloc(2*MaxCount+16);
	if (ColorChart == NULL)
		return 0;
	setColors(ColorChart,MaxCount,mand->userbands);
/*	inversesave = GlobalVar.InverseVideoFlag;
	GlobalVar.InverseVideoFlag = 0; */
	dp->clear(dp,dp->CurrentPort);
/*	GlobalVar.InverseVideoFlag = inversesave; */
	for(i = 0; i < BigX; i++, lastcount = 0, times = 0)
	{
		for(j = 0; j < BigY; j++)
		{
			if (!getevent(dp))
			{
				free(ColorChart);
				return -1;
			}
			if (dp->PortEvent.type)
			{
				free(ColorChart);
				return -1;
			}
			count = getMand(i,j);
			if (count == lastcount)
			{
				if (Trigger) times++;
			}
			else
			{
				lasty = j;
				lastcount = count;
				times = 1;
			}
			if (times > 4)
			{
				if (crawl(dp,i,lasty,count) < 0)
				{
					free(ColorChart);
					return -1;
				}
				if (fillin(dp,i,lasty,count) < 0)
				{
					free(ColorChart);
					return -1;
				}
				times = 0;
			}
		}
	}
	free(ColorChart);
	return 1;
}


#endif



