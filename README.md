# Background

RoyalFloraHolland connects growers and buyers. We provide a platform on which growers can offer their products for both direct sale and auctioning. 

During auctioning, flowers are presented in a pre-determined order to the buyers, and auctioned using an Auctioning Clock, also known as [Dutch Auctioning](https://en.wikipedia.org/wiki/Dutch_auction).

To make this possible, RoyalFloraHolland ingests, enriches and filters a lot of data. Since Auctioning is a fast process, it is imperative that all data is processed quickly and reliably.

It's imperative that our programs function correctly, self-correct when they can, and leave useful clues about what went wrong and how to fix it when they cannot recover.

If you have any questions about the assignment or need additional clarification, please contact us!

# Task

Your task is to write a program in .Net (5 or 6) that reads flower information from an input file, matches this data against a source of characteristics to produce an overview for auctioning. This is then to be displayed, sorted by Auctioning Clock, and optionally sorted by auctioning sequence. 

The input data contains an array of lots, where each lot represents a produce a grower wants to be auctioned.

An example 'lot' looks like this:

```
{
  "clockId" : "00000000-0000-0000-0000-000000000001",
  "auctionOccurrenceDate" : "2021-10-25T00:00:00+02:00",
  "auctionSupplyID" : "",
  "auctioningSequence" : "9_000241",
  "batchReference" : "",
  "characteristics" : [
     {
        "code" : "S01",
        "sortingRank" : 0,
        "value" : "019"
     }
  ],
  "comment" : "",
  "currency" : "EUR",
  "currentNumberOfPieces" : 9801,
  "groupId" : "7465694E-7620-6965-6C65-6E2069766D20",
  "id" : "041BA2C3-F6E1-404F-93F2-795665D7EE0F",
  "initialNumberOfPieces" : 9801,
  "layersPerLoadCarrier" : 1,
  "loadCarrierCode" : "1",
  "minimumPrice" : 0.02,
  "name" : "PRUNUS COMET",
  "packageTypeCode" : "577",
  "packageTypeName" : "",
  "packagesPerLayer" : 0,
  "packagesPerLoadCarrier" : 99,
  "photoUrl" : "https://beeldbankfotos.royalfloraholland.com/foto/volledig/8713783484074PAARS",
  "piecesPerPackage" : 99,
  "preSaleCurrentNumberOfPieces" : null,
  "preSaleInitialNumberOfPieces" : null,
  "preSalePriceCurrency" : null,
  "preSalePriceValue" : null,
  "productCode" : "11822",
  "productName" : "PRUNUS COMET",
  "productShortName" : "PRUNUS COMET",
  "qualityCode" : "A2",
  "supplierCertificates" : [],
  "supplierLogoUrl" : null,
  "supplierName" : "Sample Grower",
  "supplierOrganizationId" : "00000000-0000-0000-0000-000000000000",
  "supplierRelationNumber" : "99999",
  "tradeItemId" : "00000000-0000-0000-0000-000000000000",
  "tradeItemVersion" : 0
}
```

#### Fields of interest for this excercise:
 - clockId: a guid specifying the clock on which this product is to be auctioned
 - auctioningSequence: the order in which to auction this particular lot
 - characteristics: a list containing one or more properties that describe the product (see below)
 - currentNumberOfPieces: the number of flowers in this lot
 - piecesPerPackage: minimum number of flowers that can be bought

#### note:


An example of a 'characteristic' is as follows:
```
  {
     "code" : "S01",
     "translations" : [
        {
           "language" : "nl",
           "value" : "Potmaat"
        },
        {
           "language" : "en",
           "value" : "Pot size"
        },
        {
           "language": "de",
           "value": "Topfgröße"
        }
     ],
     "values" : [
        {
           "code" : "019",
           "translations" : [
              {
                 "language" : "nl",
                 "value" : "19 cm"
              },
              {
                 "language" : "en",
                 "value" : "19 cm"
              },
              {
                 "language" : "de",
                 "value" : "19 cm"
              }
           ]
        }
      ]
    }
```
---

For each lot, you should print to the console, grouped by ClockId (and optionally ordered by auctioningSequence) the following message:

productName, supplierName, packages per carrier, number of carriers, qualityCode and characteristics description and value (in NL)

Only SXX characteristics need to be displayed

i.e.:

```
PRUNUS COMET, Sample Grower, 99, 1, A2, Potmaat: 19 cm
```

Optionally, you may present a menu or a command line option to select a ClockId and, when selected, show the above output for the selected clock.

---

# Implementation

Since code is read more often than it is written, we want our projects well structured and the code easy to read.

Your program should also contain a README that contains information about the program and includes steps on how to run the program.

The files containing the lot information and characteristics (as an array of JSON objects) can be found in the Data directory.

Feel free to use open source libraries where available.

### What we're looking for
 - Correctness. If the program doesn't run correctly, it doesn't matter how beautiful or efficient it is.
 - Conciseness. Small is beautiful. Easy to read is paramount. The easier it is for someone else to come in and modify your program, the better.
 - Reliability. You should expect to handle bad or incomplete data, and expect to handle failures.
 - Unittests.

#### In addition, please answer the following questions:
 - Q: If we put this code into production, but found it too slow, or it needed to scale to many more lots, what are the first things you would think of to speed it up?

A: More RAM, avoid swap, faster CPU, change to a real database (e.g. Oracle), more cores / nodes, simpler data structure. As for timing, on this simple Celeron laptop, the parsing takes just over 1 second, and the listing about 1/5 s for over 2000 lots.

 - Q: If you need to adapt your code to process live updates to already loaded lots, what are some of the techniques you could use?

A: The C# Dictionaries, like database tables, can be updated by just adding or deleting some entries, without doing a full parse of the full data. This needs relatively simple additional parse methods in addition to those in my classes.

### How to run this program

This is a simple Visual Studio Code console application.

To build, you will need to install the NuGet package for Newtonsoft JSON.

To run and debug in VSC, just press F5.

You can also run the executable in the Bin/Debug folder, but you may have to copy the Data folder yourself.
With VisualStudio (no Code) there is a simple option to get the Data Folder in the output, but in VSC this apparently is different. The code shows warning output if Data cannot be found.

