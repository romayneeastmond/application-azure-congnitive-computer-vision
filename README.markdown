# Azure Cognitive Services Computer Vision

An ASP.NET Core project, that uses Azure Cognitive Services Computer Vision artificial intelligence API to scan images of people.

It uses an HTTP endpoint of json data that contains people data and then analyzes each image. After each image has been analyzed the data is returned back as a json object of results. The API returns categories, description, faces, imageType, tags, color, and objects VisualFeatureTypes.

## How to Use

Restore any necessary NuGet packages before building or deploying. Ensure that the settings in the ApplicationFaces.Api/appsettings.json file are changed to match the Azure subscription key and Cognitive Services endpoints. Refactor accordingly with people data or provide the service with various images.

## Known Issues

This project reads the people information across a Http Get request which creates a bottleneck in retrieving the image data. Since parses images through Azure Cognitive Services is slower than expected, it would be better to consider parallelism or a multi-threaded approach.

Additionally saving the results to a json document is less than ideal. Naturally the more images being analyzed the larger the output. Therefore saving each result to a NoSQL database would be more reliable in the long run.

###### Sample People Data

The /samples directory contains an example of raw people data being analyzed.

```
[
  {
    "Id": "{7CC1E391-F2EE-4E7D-BEE2-1009068B79C8}",
    "Url": "/romayneeastmond/",
    "Prefix": "",
    "FirstName": "Romayne",
    "MiddleName": "",
    "LastName": "Eastmond",
    "Suffix": "",
    "FullName": "Romayne Eastmond",
    "DisplayTitle": "Senior Developer",
    "Office": "",
    "EmailAddress": "",
    "PhoneNumber": "",
    "CellularPhone": "",
    "LinkedInLink": "",
    "GooglePlusLink": "",
    "TwitterLink": "",
    "ImageUrl": "romayneeastmond/fake_image_path.png",
    "LastModified": "2022-03-29"
  }
]

```

###### Sample Analyzed Results

The /samples directory contains an example of the VisualFeatureTypes data that is returned from the Computer Vision API.

```
[
  {
    "Id": "7cc1e391-f2ee-4e7d-bee2-1009068b79c8",
    "FirstName": "Romayne",
    "LastName": "Eastmond",
    "FullName": "Romayne Eastmond",
    "Url": "https://github.com/romayneeastmond/",
    "ImageUrl": "https://github.com/romayneeastmond/fake_image_path.png",
    "DisplayTitle": "Senior Developer",
    "ImageStatistics": {
      "Id": "21ce999d-ac83-4a25-87a7-d8bd0adaca0f",
      "PersonId": "7cc1e391-f2ee-4e7d-bee2-1009068b79c7",
      "ImageAccentColor": "53596F",
      "ImageDominantColorBackground": "53596F",
      "ImageDominantColorForeground": "53596F",
      "ImageDominantColors": "White",
      "Captions": [
        {
          "Name": "a man wearing glasses",
          "Description": "",
          "Confidence": 0.5429129600524902
        }
      ],
      "Categories": [
        {
          "Name": "people_",
          "Description": "",
          "Confidence": 0.34765625
        },
        {
          "Name": "people_portrait",
          "Description": "",
          "Confidence": 0.65234375
        }
      ],
      "Tags": [
        {
          "Name": "human face",
          "Description": "",
          "Confidence": 0.9962856769561768
        },
        {
          "Name": "person",
          "Description": "",
          "Confidence": 0.9904642105102539
        },
        {
          "Name": "necktie",
          "Description": "",
          "Confidence": 0.9815733432769775
        },
        {
          "Name": "clothing",
          "Description": "",
          "Confidence": 0.9736372232437134
        },
        {
          "Name": "tie",
          "Description": "",
          "Confidence": 0.9625458121299744
        }
      ],
      "Faces": [
        {
          "Name": "Male",
          "Description": "37",
          "Confidence": 0.0
        }
      ]
    }
  }
]
```

## Copyright and Ownership

All terms used are copyright to their original authors.
