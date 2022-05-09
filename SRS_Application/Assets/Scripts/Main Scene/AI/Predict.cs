using System;
using System.Diagnostics; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Predict : MonoBehaviour
{
  public static void LinearRegression (
        List<UserData> data,
        out int hour,
        out int minute
        )
    {
        //List<UserData> data = DataManage.
        double[] xVals = new double[data.Count];
        double[] yVals = new double[data.Count];

        if (data.Count == 0)
        {
          hour = 8;
          minute = 0;
          return;
        }
        for (var i = 0; i < data.Count; i++)
        {
          
          xVals[i] = i +1;
              
          yVals[i] = data[i].getValue();
          if (yVals[i] <= 5)
            yVals[i] += 24;
        }
    
        if (xVals.Length != yVals.Length)
        {
            throw new Exception("Input values should have the same length.");    
        }

        double sumOfX = 0;
        double sumOfY = 0;
        double sumOfXSq = 0;
        double sumOfYSq = 0;
        double sumCodeviates = 0;

        for (int i = 0; i < xVals.Length; i++)
        {
            var x = xVals[i];
            
            var y = yVals[i];
            sumCodeviates += x * y;
            sumOfX += x;
            sumOfY += y;
            sumOfXSq += x * x;
            sumOfYSq += y * y;
        }

        double count = xVals.Length;
        double ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
        double ssY = sumOfYSq - ((sumOfY * sumOfY) / count);

        double rNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
        double rDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
        double sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

        double meanX = sumOfX / count;
        double meanY = sumOfY / count;
        double dblR = rNumerator / Math.Sqrt(rDenom);

        double rSquared = dblR * dblR;
        double yIntercept = meanY - ((sCo / ssX) * meanX);
    
        double slope = sCo / ssX;

        // predictedVal = slope * day_of_prediction + intercept
        double predictedValue = (slope * (xVals[xVals.Length - 1] + 1)) + yIntercept;
        
        hour = (int)(predictedValue);
        minute = (int)(60*(predictedValue - (double)hour));
        //string predictedTime = hour.ToString("D2") + ":" + minute.ToString("D2");
    }

}
