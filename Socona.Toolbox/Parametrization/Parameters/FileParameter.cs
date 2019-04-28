using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Socona.ToolBox.Parametrization.Parameters
{

    public class FileParameter : Parameter<FileInfo>
    {
        ///<summary>
        /// Available file types: <see cref = "#INPUT_FILE"/> denotes an input file,
        /// <see cref = "#OUTPUT_FILE"/> denotes an output file.
        /// 
        /// @apiviz.exclude
        ///</summary>
        public enum FileType
        {
            ///<summary>
            /// Input files (i.e. read only)
            ///</summary>
            INPUT_FILE,

            ///<summary>
            /// Output files
            ///</summary>
            OUTPUT_FILE
        }

        ///<summary>
        /// The file type of this file parameter. Specifies if the file is an input of
        /// output file.
        ///</summary>
        private FileType _fileType;



        ///<summary>
        /// Constructs a file parameter with the given optionID, file type, and
        /// optional flag.
        /// 
        ///   </summary>
        ///<param name="optionID">optionID the unique id of the option</param>
        ///<param name="fileType">the file type of this file parameter</param>
        ///<param name="optional">specifies if this parameter is an optional parameter</param>
        public FileParameter(OptionAttribute optionID, FileType fileType, bool isRequired) :
            base(optionID, isRequired)
        {
            _fileType = fileType;
        }

        public override String GetValueAsString()
        {
            return Value?.FullName ?? string.Empty;
        }

        protected override bool TryParse(object obj, out FileInfo value)
        {

            if (obj is FileInfo file)
            {
                value = file;
                return true;
            }
            if (obj is string valStr)
            {

                if (!string.IsNullOrWhiteSpace(valStr))
                {
                    if (valStr[0] == '\"' && valStr[valStr.Length - 1] == '\"')
                    {
                        valStr = valStr.Substring(1, valStr.Length - 2);
                    }
                    value = new FileInfo(valStr);
                    return true;
                }
            }
            value = null;
            return false;
        }

        ///<summary> @inheritDoc </summary>


        protected override bool ValidateConstraints(FileInfo obj, bool throwIfFailed = true)
        {
            if (!base.ValidateConstraints(obj))
            {
                return false;
            }
            if (_fileType == (FileType.INPUT_FILE))
            {
                try
                {
                    if (!obj.Exists)
                    {
                        if (throwIfFailed)
                        {
                            throw new InvalidParameterValueException("Given file " + obj.DirectoryName + " for parameter \"" + Name + "\" does not exist!\n");
                        }
                        return false;
                    }
                }
                catch (UnauthorizedAccessException e)
                {
                    if (throwIfFailed)
                    {
                        throw new InvalidParameterValueException("Given file \"" + obj.DirectoryName + "\" cannot be read, access denied!\n" + e.Message);
                    }
                    return false;
                }
            }
            return true;
        }

        public override string PlaceHolder => "<file>";
    }
}
