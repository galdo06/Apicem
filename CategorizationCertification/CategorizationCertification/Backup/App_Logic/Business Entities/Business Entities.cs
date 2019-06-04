﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
//		Template updated by: Mohammad Ashraful Alam, Microsoft MVP, ASP.NET [admin@ashraful.net]
//		Modification comment: made few items mockable. (03-30-2010)
//			* Made ObjectSet properties of database context as virtual and return type as IObjectSet
//			* Made stored procedure (function import) methods virtual and return type as List<Entity>
//			* Seperate name space for data containers	
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections.Generic;//custom-code
using Eisk.Helpers;

[assembly: EdmSchemaAttribute()]
#region EDM Relationship Metadata

[assembly: EdmRelationshipAttribute("Model_Namespace", "FK_Employees_Employees", "Employees", System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(Eisk.BusinessEntities.Employee), "Employees1", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(Eisk.BusinessEntities.Employee), true)]//custom-code

#endregion

namespace Eisk.BusinessEntities{//custom-code
#region Entities

/// <summary>
/// No Metadata Documentation available.
/// </summary>
[EdmEntityTypeAttribute(NamespaceName="Model_Namespace", Name="Employee")]
[Serializable()]
[DataContractAttribute(IsReference=true)]
public partial class Employee : EntityObject
{

	
    #region Factory Method

    /// <summary>
    /// Create a new Employee object.
    /// </summary>
    /// <param name="employeeId">Initial value of the EmployeeId property.</param>
    /// <param name="lastName">Initial value of the LastName property.</param>
    /// <param name="firstName">Initial value of the FirstName property.</param>
    /// <param name="hireDate">Initial value of the HireDate property.</param>
    /// <param name="address">Initial value of the Address property.</param>
    /// <param name="country">Initial value of the Country property.</param>
    /// <param name="homePhone">Initial value of the HomePhone property.</param>
    public static Employee CreateEmployee(global::System.Int32 employeeId, global::System.String lastName, global::System.String firstName, global::System.DateTime hireDate, global::System.String address, global::System.String country, global::System.String homePhone)
    {
        Employee employee = new Employee();
        employee.EmployeeId = employeeId;
        employee.LastName = lastName;
        employee.FirstName = firstName;
        employee.HireDate = hireDate;
        employee.Address = address;
        employee.Country = country;
        employee.HomePhone = homePhone;
        return employee;
    }

    #endregion
    #region Primitive Properties

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
    [DataMemberAttribute()]
    public global::System.Int32 EmployeeId
    {
        get
        {
            return _EmployeeId;
        }
        set
        {
            if (_EmployeeId != value)
            {
                //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
    			if (value.IsInvalidKey()) BusinessLayerHelper.ThrowErrorForInvalidDataKey("EmployeeId");
    
    
    			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
    			
    			OnEmployeeIdChanging(value);
                ReportPropertyChanging("EmployeeId");
                _EmployeeId = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("EmployeeId");
                OnEmployeeIdChanged();
            }
        }
    }
    private global::System.Int32 _EmployeeId;
    partial void OnEmployeeIdChanging(global::System.Int32 value);
    partial void OnEmployeeIdChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
    [DataMemberAttribute()]
    public global::System.String LastName
    {
        get
        {
            return _LastName;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("LastName");

			else if (value.IsNull()) BusinessLayerHelper.ThrowErrorForNullValue("LastName");

			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnLastNameChanging(value);
            ReportPropertyChanging("LastName");
            _LastName = StructuralObject.SetValidValue(value, false);
            ReportPropertyChanged("LastName");
            OnLastNameChanged();
        }
    }
    private global::System.String _LastName;
    partial void OnLastNameChanging(global::System.String value);
    partial void OnLastNameChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
    [DataMemberAttribute()]
    public global::System.String FirstName
    {
        get
        {
            return _FirstName;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("FirstName");

			else if (value.IsNull()) BusinessLayerHelper.ThrowErrorForNullValue("FirstName");

			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnFirstNameChanging(value);
            ReportPropertyChanging("FirstName");
            _FirstName = StructuralObject.SetValidValue(value, false);
            ReportPropertyChanged("FirstName");
            OnFirstNameChanged();
        }
    }
    private global::System.String _FirstName;
    partial void OnFirstNameChanging(global::System.String value);
    partial void OnFirstNameChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
    [DataMemberAttribute()]
    public global::System.String Title
    {
        get
        {
            return _Title;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("Title");


			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnTitleChanging(value);
            ReportPropertyChanging("Title");
            _Title = StructuralObject.SetValidValue(value, true);
            ReportPropertyChanged("Title");
            OnTitleChanged();
        }
    }
    private global::System.String _Title;
    partial void OnTitleChanging(global::System.String value);
    partial void OnTitleChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
    [DataMemberAttribute()]
    public global::System.String TitleOfCourtesy
    {
        get
        {
            return _TitleOfCourtesy;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("TitleOfCourtesy");


			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnTitleOfCourtesyChanging(value);
            ReportPropertyChanging("TitleOfCourtesy");
            _TitleOfCourtesy = StructuralObject.SetValidValue(value, true);
            ReportPropertyChanged("TitleOfCourtesy");
            OnTitleOfCourtesyChanged();
        }
    }
    private global::System.String _TitleOfCourtesy;
    partial void OnTitleOfCourtesyChanging(global::System.String value);
    partial void OnTitleOfCourtesyChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
    [DataMemberAttribute()]
    public Nullable<global::System.DateTime> BirthDate
    {
        get
        {
            return _BirthDate;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("BirthDate");


			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnBirthDateChanging(value);
            ReportPropertyChanging("BirthDate");
            _BirthDate = StructuralObject.SetValidValue(value);
            ReportPropertyChanged("BirthDate");
            OnBirthDateChanged();
        }
    }
    private Nullable<global::System.DateTime> _BirthDate;
    partial void OnBirthDateChanging(Nullable<global::System.DateTime> value);
    partial void OnBirthDateChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
    [DataMemberAttribute()]
    public global::System.DateTime HireDate
    {
        get
        {
            return _HireDate;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("HireDate");


			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnHireDateChanging(value);
            ReportPropertyChanging("HireDate");
            _HireDate = StructuralObject.SetValidValue(value);
            ReportPropertyChanged("HireDate");
            OnHireDateChanged();
        }
    }
    private global::System.DateTime _HireDate;
    partial void OnHireDateChanging(global::System.DateTime value);
    partial void OnHireDateChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
    [DataMemberAttribute()]
    public global::System.String Address
    {
        get
        {
            return _Address;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("Address");

			else if (value.IsNull()) BusinessLayerHelper.ThrowErrorForNullValue("Address");

			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnAddressChanging(value);
            ReportPropertyChanging("Address");
            _Address = StructuralObject.SetValidValue(value, false);
            ReportPropertyChanged("Address");
            OnAddressChanged();
        }
    }
    private global::System.String _Address;
    partial void OnAddressChanging(global::System.String value);
    partial void OnAddressChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
    [DataMemberAttribute()]
    public global::System.String City
    {
        get
        {
            return _City;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("City");


			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnCityChanging(value);
            ReportPropertyChanging("City");
            _City = StructuralObject.SetValidValue(value, true);
            ReportPropertyChanged("City");
            OnCityChanged();
        }
    }
    private global::System.String _City;
    partial void OnCityChanging(global::System.String value);
    partial void OnCityChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
    [DataMemberAttribute()]
    public global::System.String Region
    {
        get
        {
            return _Region;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("Region");


			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnRegionChanging(value);
            ReportPropertyChanging("Region");
            _Region = StructuralObject.SetValidValue(value, true);
            ReportPropertyChanged("Region");
            OnRegionChanged();
        }
    }
    private global::System.String _Region;
    partial void OnRegionChanging(global::System.String value);
    partial void OnRegionChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
    [DataMemberAttribute()]
    public global::System.String PostalCode
    {
        get
        {
            return _PostalCode;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("PostalCode");


			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnPostalCodeChanging(value);
            ReportPropertyChanging("PostalCode");
            _PostalCode = StructuralObject.SetValidValue(value, true);
            ReportPropertyChanged("PostalCode");
            OnPostalCodeChanged();
        }
    }
    private global::System.String _PostalCode;
    partial void OnPostalCodeChanging(global::System.String value);
    partial void OnPostalCodeChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
    [DataMemberAttribute()]
    public global::System.String Country
    {
        get
        {
            return _Country;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("Country");

			else if (value.IsNull()) BusinessLayerHelper.ThrowErrorForNullValue("Country");

			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnCountryChanging(value);
            ReportPropertyChanging("Country");
            _Country = StructuralObject.SetValidValue(value, false);
            ReportPropertyChanged("Country");
            OnCountryChanged();
        }
    }
    private global::System.String _Country;
    partial void OnCountryChanging(global::System.String value);
    partial void OnCountryChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
    [DataMemberAttribute()]
    public global::System.String HomePhone
    {
        get
        {
            return _HomePhone;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("HomePhone");

			else if (value.IsNull()) BusinessLayerHelper.ThrowErrorForNullValue("HomePhone");

			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnHomePhoneChanging(value);
            ReportPropertyChanging("HomePhone");
            _HomePhone = StructuralObject.SetValidValue(value, false);
            ReportPropertyChanged("HomePhone");
            OnHomePhoneChanged();
        }
    }
    private global::System.String _HomePhone;
    partial void OnHomePhoneChanging(global::System.String value);
    partial void OnHomePhoneChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
    [DataMemberAttribute()]
    public global::System.String Extension
    {
        get
        {
            return _Extension;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("Extension");


			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnExtensionChanging(value);
            ReportPropertyChanging("Extension");
            _Extension = StructuralObject.SetValidValue(value, true);
            ReportPropertyChanged("Extension");
            OnExtensionChanged();
        }
    }
    private global::System.String _Extension;
    partial void OnExtensionChanging(global::System.String value);
    partial void OnExtensionChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
    [DataMemberAttribute()]
    public global::System.Byte[] Photo
    {
        get
        {
            return StructuralObject.GetValidValue(_Photo);
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnPhotoChanging(value);
            ReportPropertyChanging("Photo");
            _Photo = StructuralObject.SetValidValue(value, true);
            ReportPropertyChanged("Photo");
            OnPhotoChanged();
        }
    }
    private global::System.Byte[] _Photo;
    partial void OnPhotoChanging(global::System.Byte[] value);
    partial void OnPhotoChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
    [DataMemberAttribute()]
    public global::System.String Notes
    {
        get
        {
            return _Notes;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("Notes");


			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnNotesChanging(value);
            ReportPropertyChanging("Notes");
            _Notes = StructuralObject.SetValidValue(value, true);
            ReportPropertyChanged("Notes");
            OnNotesChanged();
        }
    }
    private global::System.String _Notes;
    partial void OnNotesChanging(global::System.String value);
    partial void OnNotesChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
    [DataMemberAttribute()]
    public Nullable<global::System.Int32> ReportsTo
    {
        get
        {
            return _ReportsTo;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			if (value.IsEmpty()) BusinessLayerHelper.ThrowErrorForEmptyValue("ReportsTo");


			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnReportsToChanging(value);
            ReportPropertyChanging("ReportsTo");
            _ReportsTo = StructuralObject.SetValidValue(value);
            ReportPropertyChanged("ReportsTo");
            OnReportsToChanged();
        }
    }
    private Nullable<global::System.Int32> _ReportsTo;
    partial void OnReportsToChanging(Nullable<global::System.Int32> value);
    partial void OnReportsToChanged();

    #endregion

    #region Navigation Properties

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [XmlIgnoreAttribute()]
    [SoapIgnoreAttribute()]
    [DataMemberAttribute()]
    [EdmRelationshipNavigationPropertyAttribute("Model_Namespace", "FK_Employees_Employees", "Employees1")]
    public EntityCollection<Employee> Subordinates
    {
        get
        {
            return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<Employee>("Model_Namespace.FK_Employees_Employees", "Employees1");
        }
        set
        {
            if ((value != null))
            {
                ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<Employee>("Model_Namespace.FK_Employees_Employees", "Employees1", value);
            }
        }
    }

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [XmlIgnoreAttribute()]
    [SoapIgnoreAttribute()]
    [DataMemberAttribute()]
    [EdmRelationshipNavigationPropertyAttribute("Model_Namespace", "FK_Employees_Employees", "Employees")]
    public Employee Supervisor
    {
        get
        {
            return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Employee>("Model_Namespace.FK_Employees_Employees", "Employees").Value;
        }
        set
        {
            ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Employee>("Model_Namespace.FK_Employees_Employees", "Employees").Value = value;
        }
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [BrowsableAttribute(false)]
    [DataMemberAttribute()]
    public EntityReference<Employee> SupervisorReference
    {
        get
        {
            return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Employee>("Model_Namespace.FK_Employees_Employees", "Employees");
        }
        set
        {
            if ((value != null))
            {
                ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<Employee>("Model_Namespace.FK_Employees_Employees", "Employees", value);
            }
        }
    }

    #endregion
}

#endregion
#region ComplexTypes

/// <summary>
/// No Metadata Documentation available.
/// </summary>
[EdmComplexTypeAttribute(NamespaceName="Model_Namespace", Name="EmployeeWithSupervisorName")]
[DataContractAttribute(IsReference=true)]
[Serializable()]
public partial class EmployeeWithSupervisorName : ComplexObject
{
    #region Factory Method

    /// <summary>
    /// Create a new EmployeeWithSupervisorName object.
    /// </summary>
    /// <param name="employeeID">Initial value of the EmployeeID property.</param>
    /// <param name="firstName">Initial value of the FirstName property.</param>
    /// <param name="lastName">Initial value of the LastName property.</param>
    /// <param name="supervisorFirstName">Initial value of the SupervisorFirstName property.</param>
    /// <param name="supervisorLastName">Initial value of the SupervisorLastName property.</param>
    public static EmployeeWithSupervisorName CreateEmployeeWithSupervisorName(global::System.Int32 employeeID, global::System.String firstName, global::System.String lastName, global::System.String supervisorFirstName, global::System.String supervisorLastName)
    {
        EmployeeWithSupervisorName employeeWithSupervisorName = new EmployeeWithSupervisorName();
        employeeWithSupervisorName.EmployeeID = employeeID;
        employeeWithSupervisorName.FirstName = firstName;
        employeeWithSupervisorName.LastName = lastName;
        employeeWithSupervisorName.SupervisorFirstName = supervisorFirstName;
        employeeWithSupervisorName.SupervisorLastName = supervisorLastName;
        return employeeWithSupervisorName;
    }

    #endregion
    #region Primitive Properties

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
    [DataMemberAttribute()]
    public global::System.Int32 EmployeeID
    {
        get
        {
            return _EmployeeID;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnEmployeeIDChanging(value);
            ReportPropertyChanging("EmployeeID");
            _EmployeeID = StructuralObject.SetValidValue(value);
            ReportPropertyChanged("EmployeeID");
            OnEmployeeIDChanged();
        }
    }
    private global::System.Int32 _EmployeeID;
    partial void OnEmployeeIDChanging(global::System.Int32 value);
    partial void OnEmployeeIDChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
    [DataMemberAttribute()]
    public global::System.String FirstName
    {
        get
        {
            return _FirstName;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnFirstNameChanging(value);
            ReportPropertyChanging("FirstName");
            _FirstName = StructuralObject.SetValidValue(value, false);
            ReportPropertyChanged("FirstName");
            OnFirstNameChanged();
        }
    }
    private global::System.String _FirstName;
    partial void OnFirstNameChanging(global::System.String value);
    partial void OnFirstNameChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
    [DataMemberAttribute()]
    public global::System.String LastName
    {
        get
        {
            return _LastName;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnLastNameChanging(value);
            ReportPropertyChanging("LastName");
            _LastName = StructuralObject.SetValidValue(value, false);
            ReportPropertyChanged("LastName");
            OnLastNameChanged();
        }
    }
    private global::System.String _LastName;
    partial void OnLastNameChanging(global::System.String value);
    partial void OnLastNameChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
    [DataMemberAttribute()]
    public global::System.String SupervisorFirstName
    {
        get
        {
            return _SupervisorFirstName;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnSupervisorFirstNameChanging(value);
            ReportPropertyChanging("SupervisorFirstName");
            _SupervisorFirstName = StructuralObject.SetValidValue(value, false);
            ReportPropertyChanged("SupervisorFirstName");
            OnSupervisorFirstNameChanged();
        }
    }
    private global::System.String _SupervisorFirstName;
    partial void OnSupervisorFirstNameChanging(global::System.String value);
    partial void OnSupervisorFirstNameChanged();

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
    [DataMemberAttribute()]
    public global::System.String SupervisorLastName
    {
        get
        {
            return _SupervisorLastName;
        }
        set
        {
            //-- Logic for Null and Empty Value Validation [CUSTOM CODE]
			
			//-- Logic for Null and Empty Value Validation Ends [CUSTOM CODE]
			
			OnSupervisorLastNameChanging(value);
            ReportPropertyChanging("SupervisorLastName");
            _SupervisorLastName = StructuralObject.SetValidValue(value, false);
            ReportPropertyChanged("SupervisorLastName");
            OnSupervisorLastNameChanged();
        }
    }
    private global::System.String _SupervisorLastName;
    partial void OnSupervisorLastNameChanging(global::System.String value);
    partial void OnSupervisorLastNameChanged();

    #endregion
}

#endregion


}
