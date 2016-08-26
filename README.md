# SPExtendedWPS
SharePoint 2013 Extended Content Search Web Part

Project Description:
This web part extends the OOTB SharePoint Content Search Web Part to allow you to filter by target audience.

The Target Audience that is being used in this solution is the audience groups you set up in Central Administration.

Then in your Search Schema also in Central Administration, you need to make sure you have an entry for "TargetAudience" with the crawled properties mapped to "Audience, ows_Audience, ows_Target_x0020_Audiences".

Once you deploy the solution, you have to configure the web part to filter the audience by adding in TargetAudienceQuery.  This will filter off of the content's column titled TargetAudience that you set in the library of the content.  The only downfall to this is that you cannot test the query. No results will show.  After you click OK, results will appear and you can test by signing in as another user that is in each of those audience groups you have created.  I usually use a test account I've created that belongs to each of my audiences to test the results.


Special thanks to Srikanth Tiyyaguru who wrote this article:  http://www.codeproject.com/Tips/799307/Extend-Content-Search-web-part-to-add-Target-Audie

This gave me a good start! His solution goes off of the user's sharepoint groups if you need to filter off of that instead.  I had to use audiences.
