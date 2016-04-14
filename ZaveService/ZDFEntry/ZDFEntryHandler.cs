using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaveModel.ZDF;
using ZaveGlobalSettings.Data_Structures;

namespace ZaveService.ZDFEntry
{
    public abstract class ZDFEntryHandler
    {
        protected SelectionState _modelState;
        protected IZDF _repository;

        public ZDFEntryHandler(SelectionState modelState, IZDF repository)
        {
            _modelState = modelState;
            _repository = repository;
        }

        public bool ValidateZDFEntry(ZaveModel.ZDFEntry.IZDFEntry entryToValidate)
        {
            if (entryToValidate.Source.DocText.Trim().Length == 0)
            {
                _modelState.AddError("Highlighted Text", "Highlighted Text is required");
            }
                return _modelState.IsValid;
            
        }

        public bool CreateZDFEntry(ZaveModel.ZDFEntry.IZDFEntry entryToCreate)
        {
            if (!ValidateZDFEntry(entryToCreate))
                return false;

            try
            {
                _repository.Add(entryToCreate);
            }
            catch
            {
                return false;
            }
            return true;
        }

        
    }
}
