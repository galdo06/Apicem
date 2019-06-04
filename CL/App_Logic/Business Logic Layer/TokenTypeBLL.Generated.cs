//------------------------------------------------------------------------------
// <auto-generated>
//
// ***************** Copyright Notice *****************
// 
// This code is licensed under Microsoft Public License (Ms-PL). 
// You are free to use, modify and distribute any portion of this code. 
// The only requirement to do that, you need to keep the developer name, as provided below to recognize and encourage original work:
//
// =======================================================
//   
// Architecture Designed and Implemented By:
// Mohammad Ashraful Alam
// Microsoft Most Valuable Professional, ASP.NET 2007 – 2011
// Twitter: http://twitter.com/AshrafulAlam | Blog: http://blog.ashraful.net | Portfolio: http://www.ashraful.net
//   
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using Eisk.BusinessEntities;
using Eisk.DataAccessLayer;
using Eisk.Helpers;

namespace Eisk.BusinessLogicLayer
{   
	[System.ComponentModel.DataObject(true)]
	public partial class TokenTypeBLL:IDisposable
	{
		#region Constructors, Dependency and Partial Method Declaration

        public TokenTypeBLL() : this(new DatabaseContext()) { }

        public TokenTypeBLL(DatabaseContext DatabaseContext)
        {
            _DatabaseContext = DatabaseContext;
        }

        DatabaseContext _DatabaseContext;

        public void Dispose()
        {
            if (_DatabaseContext != null)
            {
                _DatabaseContext.Dispose();
                _DatabaseContext = null;
            }
            
            GC.SuppressFinalize(this);
        }

        partial void OnTokenTypeSaving(TokenType tokenType);

        partial void OnTokenTypeCreating(TokenType tokenType);
        partial void OnTokenTypeCreated(TokenType tokenType);

        partial void OnTokenTypeUpdating(TokenType tokenType);
        partial void OnTokenTypeUpdated(TokenType tokenType);

        partial void OnTokenTypeSaved(TokenType tokenType);

        partial void OnTokenTypeDeleting(TokenType tokenType);
        partial void OnTokenTypeDeleted(TokenType tokenType);


        #endregion

        #region Get Methods

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public TokenType GetTokenTypeByTokenTypeID(int tokenTypeID)
        {
            //Validate Input
            if (tokenTypeID.IsInvalidKey())
               BusinessLayerHelper.ThrowErrorForInvalidDataKey("tokenTypeID");
            return (_DatabaseContext.TokenTypes.FirstOrDefault(tokenType => tokenType.TokenTypeID == tokenTypeID));
        }
			
				
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<TokenType> GetAllTokenTypes()
        {
            return _DatabaseContext.TokenTypes.ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<TokenType> GetAllTokenTypesPaged(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "TokenTypeID";
				
			if (startRowIndex < 0) 
				throw (new ArgumentOutOfRangeException("startRowIndex"));
				
			if (maximumRows < 0) 
				throw (new ArgumentOutOfRangeException("maximumRows"));
			
            return (
                    from tokenTypeList in 
                        _DatabaseContext.TokenTypes
                        .DynamicOrderBy(orderBy)
                    select tokenTypeList
                    )
                    .Skip(startRowIndex)
                    .Take(maximumRows)
                    .ToList();
        }

        public int GetTotalCountForAllTokenTypes(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            return _DatabaseContext.TokenTypes.Count();
        }

        #endregion

        #region Persistence (Create, Update, Delete) Methods

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)]
        public virtual int CreateNewTokenType(TokenType newTokenType)
        {
            // Validate Parameters 
            if (newTokenType == null)
                throw (new ArgumentNullException("newTokenType"));

	        // Apply business rules
            OnTokenTypeSaving(newTokenType);
            OnTokenTypeCreating(newTokenType);

            _DatabaseContext.TokenTypes.AddObject(newTokenType);
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No tokenType created!");

            // Apply business workflow
            OnTokenTypeCreated(newTokenType);
            OnTokenTypeSaved(newTokenType);

            return newTokenType.TokenTypeID;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, true)]
        public void UpdateTokenType(TokenType updatedTokenType)
        {
            // Validate Parameters
            if (updatedTokenType == null)
                throw (new ArgumentNullException("updatedTokenType"));

            // Validate Primary key value
            if (updatedTokenType.TokenTypeID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("TokenTypeID");

            // Apply business rules
            OnTokenTypeSaving(updatedTokenType);
            OnTokenTypeUpdating(updatedTokenType);

            //attaching and making ready for parsistance
            if (updatedTokenType.EntityState == EntityState.Detached)
                _DatabaseContext.TokenTypes.Attach(updatedTokenType);
			_DatabaseContext.ObjectStateManager.ChangeObjectState(updatedTokenType, System.Data.EntityState.Modified);//this line makes the code un-testable!
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No tokenType updated!");

            //Apply business workflow
            OnTokenTypeUpdated(updatedTokenType);
            OnTokenTypeSaved(updatedTokenType);

        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, true)]
        public void DeleteTokenType(TokenType tokenTypeToBeDeleted)
        {
            //Validate Input
            if (tokenTypeToBeDeleted == null)
                throw (new ArgumentNullException("tokenTypeToBeDeleted"));

            // Validate Primary key value
            if (tokenTypeToBeDeleted.TokenTypeID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("TokenTypeID");

            OnTokenTypeSaving(tokenTypeToBeDeleted);
            OnTokenTypeDeleting(tokenTypeToBeDeleted);

            if (tokenTypeToBeDeleted.EntityState == EntityState.Detached)
             _DatabaseContext.TokenTypes.Attach(tokenTypeToBeDeleted);
			_DatabaseContext.TokenTypes.DeleteObject(tokenTypeToBeDeleted);
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No TokenType deleted!");
            
            OnTokenTypeDeleted(tokenTypeToBeDeleted);
            OnTokenTypeSaved(tokenTypeToBeDeleted);

        }

        public void DeleteTokenTypes(List<int> tokenTypeIDsToDelete)
        {
            //Validate Input
            foreach (int tokenTypeID in tokenTypeIDsToDelete)
                if (tokenTypeID.IsInvalidKey())
                    BusinessLayerHelper.ThrowErrorForInvalidDataKey("TokenTypeID");

            List<TokenType> tokenTypesToBeDeleted = new List<TokenType>();

            foreach (int tokenTypeID in tokenTypeIDsToDelete)
            {
                TokenType tokenType = new TokenType { TokenTypeID = tokenTypeID };
                _DatabaseContext.TokenTypes.Attach(tokenType);
				_DatabaseContext.TokenTypes.DeleteObject(tokenType);
                tokenTypesToBeDeleted.Add(tokenType);
                OnTokenTypeDeleting(tokenType);
            }

            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows != tokenTypeIDsToDelete.Count) 
                throw new DataNotUpdatedException("One or more tokenType records have not been deleted.");
            foreach (TokenType tokenTypeToBeDeleted in tokenTypesToBeDeleted)
                OnTokenTypeDeleted(tokenTypeToBeDeleted);
        }

        #endregion
	
	}
}
