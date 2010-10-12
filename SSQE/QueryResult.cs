/*
 * Copyright (c) 2010, Vitaliy Litvinenko
 * All rights reserved.
 * Find me on http://beardeddev.pp.ua
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace SSQE
{
    /// <summary>
    /// Dynamic query result class
    /// </summary>
    public class QueryResult : DynamicObject
    {
        /// <summary>
        /// Values holder dictionary
        /// </summary>
        private Dictionary<string, object> properties;

        /// <summary>
        /// Provides the implementation for operations that set member values. 
        /// Classes derived from the DynamicObject class can override this method to specify dynamic behavior for operations such as setting 
        /// a value for a property.
        /// </summary>
        /// <param name="binder">
        /// Provides information about the object that called the dynamic operation. 
        /// The binder.Name property provides the name of the member to which the value is being assigned. 
        /// For example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the 
        /// DynamicObject class, binder.Name returns "SampleProperty". 
        /// The binder.IgnoreCase property specifies whether the member name is case-sensitive.
        /// </param>
        /// <param name="value">
        /// he value to set to the member. For example, for sampleObject.SampleProperty = "Test",
        /// where sampleObject is an instance of the class derived from the DynamicObject class, the value is "Test".
        /// </param>
        /// <returns>
        /// rue if the operation is successful; otherwise, false. 
        /// If this method returns false, the run-time binder of the language determines the behavior. 
        /// (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            properties[binder.Name] = value;
            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that get member values. 
        /// Classes derived from the DynamicObject class can override this method to specify dynamic behavior 
        /// for operations such as getting a value for a property.
        /// </summary>
        /// <param name="binder">
        /// Provides information about the object that called the dynamic operation. 
        /// The binder.Name property provides the name of the member on which the dynamic operation is performed. 
        /// For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, 
        /// where sampleObject is an instance of the class derived from the DynamicObject class, 
        /// binder.Name returns "SampleProperty". 
        /// The binder.IgnoreCase property specifies whether the member name is case-sensitive.
        /// </param>
        /// <param name="result">
        /// The result of the get operation. For example, if the method is called for a property, you can assign the property value to result.
        /// </param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. 
        /// If this method returns false, the run-time binder of the language determines the behavior. 
        /// (In most cases, a run-time exception is thrown.)
        /// </returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = properties[binder.Name];
 	        return true;
        }

        /// <summary>
        /// Creates new instance of query result dynamic object
        /// </summary>
        public QueryResult()
        {
            properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Set the properties from exturnal dictionary
        /// </summary>
        /// <param name="properties"></param>
        internal QueryResult(Dictionary<string, object> properties)
        {
            this.properties = properties;
        }
    }
}
