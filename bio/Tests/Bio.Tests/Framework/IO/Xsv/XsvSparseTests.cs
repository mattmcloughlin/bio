﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.Algorithms.Assembly;
using Bio.IO.Xsv;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bio.Tests.IO.Xsv
{
    /// <summary>
    /// Tests for the XsvSparse classes.
    /// </summary>
    [TestClass]
    public class XsvSparseTests
    {
        private const string XsvFilename = @"\TestUtils\SampleSparseSeq.csv";

        /// <summary>
        /// Validate xsv parser for filepath
        /// Input : XsvSparse File
        /// Validation : Expected sequence, Sequence Alphabet, Sequence ID.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparserParserValidateParseFilePath()
        {
            XsvSparseParserGeneralTestCases();
        }

        /// <summary>
        /// Validate xsv formatter for filepath
        /// Input : XsvSparse File
        /// Validation : Format is successful.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseFormatterValidateFilePath()
        {
            XsvSparseFormatterGeneralTestCases("FormatFilePath");
        }

        /// <summary>
        /// Validate xsv formatter for one filepath
        /// Input : XsvSparse File
        /// Validation : Format is successful.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseFormatterValidateFilePathWithSeqList()
        {
            XsvSparseFormatterGeneralTestCases("ForamtListWithFilePath");
        }

        /// <summary>
        /// Validate All properties in XsvSparse formatter class
        /// Input : One line sequence and update all properties
        /// Validation : Validate the properties
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseFormatterProperties()
        {
            string XsvTempFileName = Path.GetTempFileName();
            using (XsvSparseFormatter formatterObj = new XsvSparseFormatter(XsvTempFileName, ',', '#'))
            {
                Assert.AreEqual("Sparse Sequence formatter to character separated value file", formatterObj.Description);
                Assert.AreEqual("csv,tsv", formatterObj.SupportedFileTypes);
                Assert.AreEqual("XsvSparseFormatter", formatterObj.Name);
                Assert.AreEqual(',', formatterObj.Separator);
                Assert.AreEqual('#', formatterObj.SequenceIDPrefix);
            }

            if (File.Exists(XsvTempFileName))
                File.Delete(XsvTempFileName);
        }

        /// <summary>
        /// Validate SparseContigFormatter
        /// Input : Xsv file.
        /// Validation : Validation of Format() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvContigFormatter()
        {
            // Gets the expected sequence from the Xml
            string filePathObj = Directory.GetCurrentDirectory() + XsvFilename;
            string XsvTempFileName = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(filePathObj));

            XsvContigParser parserObj = new XsvContigParser(filePathObj, Alphabets.DNA, ',', '#');

            Contig contig, expectedContig;

            contig = parserObj.ParseContig();
            string seqId = string.Empty;
            foreach (Contig.AssembledSequence seq in contig.Sequences)
            {
                seqId += seq.Sequence.ID + ",";
            }

            // Format Xsv file.
            XsvContigFormatter formatObj = new XsvContigFormatter(XsvTempFileName, ',', '#');

            formatObj.Write(contig);
            formatObj.Close();
            formatObj.Dispose();

            XsvContigParser parserObj1 = new XsvContigParser(XsvTempFileName, Alphabets.DNA, ',', '#');

            expectedContig = parserObj1.ParseContig();
            string expectedseqId = string.Empty;
            foreach (Contig.AssembledSequence seq in expectedContig.Sequences)
            {
                expectedseqId += seq.Sequence.ID + ",";
            }

            // Validate parsed temp file with original Xsv file.
            Assert.AreEqual(contig.Length, expectedContig.Length);
            Assert.AreEqual(contig.Consensus.Count, expectedContig.Consensus.Count);
            Assert.AreEqual(contig.Consensus.ID, expectedContig.Consensus.ID);
            Assert.AreEqual(contig.Sequences.Count, expectedContig.Sequences.Count);
            Assert.AreEqual(seqId.Length, expectedseqId.Length);
            Assert.AreEqual(seqId, expectedseqId);
        }

        /// <summary>
        /// XsvSparse parser generic method called by all the test cases 
        /// to validate the test case based on the parameters passed.
        /// </summary>
        private void XsvSparseParserGeneralTestCases()
        {
            // Gets the expected sequence from the Xml
            string filePathObj = Directory.GetCurrentDirectory() + XsvFilename;

            Assert.IsTrue(File.Exists(filePathObj));
            // Logs information to the log file

            IEnumerable<ISequence> seqList = null;            
            SparseSequence sparseSeq = null;
            XsvContigParser parserObj = new XsvContigParser(filePathObj, Alphabets.DNA, ',', '#');
            string expectedSeqIds = "Chr22+Chr22+Chr22+Chr22,m;Chr22;16,m;Chr22;17,m;Chr22;29,m;Chr22;32,m;Chr22;39,m;Chr22;54,m;Chr22;72,m;Chr22;82,m;Chr22;85,m;Chr22;96,m;Chr22;99,m;Chr22;118,m;Chr22;119,m;Chr22;129,m;Chr22;136,m;Chr22;146,m;Chr22;153,m;Chr22;161,m;Chr22;162,m;Chr22;174,m;Chr22;183,m;Chr22;209,m;Chr22;210,m;Chr22;224,m;Chr22;241,m;Chr22;243,m;Chr22;253,m;Chr22;267,m;Chr22;309,m;Chr22;310,m;Chr22;313,m;Chr22;331,m;Chr22;333,m;Chr22;338,m;Chr22;348,m;Chr22;352,m;Chr22;355,m;Chr22;357,m;Chr22;368,m;Chr22;370,m;Chr22;380,m;Chr22;382,m;Chr22;402,m;Chr22;418,m;Chr22;419,m;Chr22;429,m;Chr22;432,m;Chr22;450,m;Chr22;462,m;Chr22;482,m;Chr22;484,m;Chr22;485,m;Chr22;494,m;Chr22;508,m;Chr22;509,m;Chr22;512,";
                    seqList = parserObj.Parse();
                    sparseSeq = (SparseSequence)seqList.FirstOrDefault();
                 
            if (null == sparseSeq)
            {
                string expCount = "57";
                Assert.IsNotNull(seqList);
                Assert.AreEqual(expCount, seqList.ToList().Count);

                StringBuilder actualId = new StringBuilder();
                foreach (ISequence seq in seqList)
                {
                    SparseSequence sps = (SparseSequence)seq;
                    actualId.Append(sps.ID);
                    actualId.Append(",");
                }

                Assert.AreEqual(expectedSeqIds, actualId.ToString());
            }
            else
            {
                string[] idArray = expectedSeqIds.Split(',');
                Assert.AreEqual(sparseSeq.ID, idArray[0]);
            }

            string XsvTempFileName = Path.GetTempFileName();

            using (XsvSparseFormatter formatter = new XsvSparseFormatter(XsvTempFileName, ',', '#'))
            {
                formatter.Write(seqList.ToList());
            }

            string expectedOutput = string.Empty;
            using (StreamReader readerSource = new StreamReader(filePathObj))
            {
                expectedOutput = readerSource.ReadToEnd();
            }

            string actualOutput = string.Empty;
            using (StreamReader readerDest = new StreamReader(XsvTempFileName))
            {
                actualOutput = readerDest.ReadToEnd();
            }
           
            Assert.AreEqual(expectedOutput, actualOutput);


            Assert.IsNotNull(sparseSeq.Alphabet);
            // Delete the temporary file.
            if (File.Exists(XsvTempFileName))
                File.Delete(XsvTempFileName);
        }

        /// <summary>
        /// XsvSparse formatter generic method called by all the test cases 
        /// to validate the test case based on the parameters passed.
        /// </summary>
        /// <param name="switchParam">Additional parameter 
        /// based on which the validation of  test case is done.</param>
        private void XsvSparseFormatterGeneralTestCases(string switchParam)
        {
            // Gets the expected sequence from the Xml
            string filePathObj = Directory.GetCurrentDirectory() + XsvFilename;

            Assert.IsTrue(File.Exists(filePathObj));

            IEnumerable<ISequence> seqList = null;
            SparseSequence sparseSeq = null;
            XsvContigParser parserObj = new XsvContigParser(filePathObj, Alphabets.DNA, ',', '#');
            seqList = parserObj.Parse();
            sparseSeq = (SparseSequence)seqList.FirstOrDefault();

            IList<IndexedItem<byte>> sparseSeqItems =
               sparseSeq.GetKnownSequenceItems();

            string XsvTempFileName = Path.GetTempFileName();
            using (XsvSparseFormatter formatterObj = new XsvSparseFormatter(XsvTempFileName, ',', '#'))
            {
                switch (switchParam)
                {
                    case "FormatFilePath":
                        formatterObj.Write(sparseSeq);
                        break;
                    default:
                        break;
                    case "ForamtListWithFilePath":
                        formatterObj.Write(sparseSeq);
                        break;
                }
            }

            // Parse a formatted Xsv file and validate.
            using (XsvContigParser parserObj1 = new XsvContigParser(XsvTempFileName, Alphabets.DNA, ',', '#'))
            {
                SparseSequence expectedSeq;
                seqList = parserObj1.Parse();


                expectedSeq = (SparseSequence)seqList.FirstOrDefault();

                IList<IndexedItem<byte>> expectedSparseSeqItems =
                    expectedSeq.GetKnownSequenceItems();

                Assert.AreEqual(sparseSeqItems.Count, expectedSparseSeqItems.Count);
                for (int i = 0; i < sparseSeqItems.Count; i++)
                {
                    Assert.AreEqual(sparseSeqItems.ElementAt(i).Index, expectedSparseSeqItems.ElementAt(i).Index);
                    Assert.AreEqual(sparseSeqItems.ElementAt(i).Item, expectedSparseSeqItems.ElementAt(i).Item);
                }

            }

            // Delete the temporary file.
            if (File.Exists(XsvTempFileName))
                File.Delete(XsvTempFileName);
        }
    }
}