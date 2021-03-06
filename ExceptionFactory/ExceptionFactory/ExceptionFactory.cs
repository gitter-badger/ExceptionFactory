﻿using System;
using System.Collections.Generic;

namespace Sullinger.ExceptionFactory
{
    /// <summary>
    /// Provides helper methods for quickly throwing exceptions based on conditions.
    /// </summary>
    public static class ExceptionFactory
    {
        /// <summary>
        /// Throws the exception if predicate is true.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">The message.</param>
        /// <param name="character">The character.</param>
        /// <param name="data">The data.</param>
        public static ExceptionFactoryResult ThrowExceptionIf<TException>(Func<bool> predicate, string message = null, params KeyValuePair<string, string>[] data) where TException : Exception, new()
        {
            return ThrowExceptionIf<TException>(predicate(), message, data);
        }

        /// <summary>
        /// Throws the exception given by the delegate if predicate is true.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="character">The character.</param>
        /// <param name="data">The data.</param>
        public static ExceptionFactoryResult ThrowExceptionIf<TException>(Func<bool> predicate, Func<TException> exception, params KeyValuePair<string, string>[] data) where TException : Exception, new()
        {
            return ThrowExceptionIf<TException>(predicate(), exception, data);
        }

        /// <summary>
        /// Throws the exception if condition is true.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <param name="message">The message.</param>
        /// <param name="character">The character.</param>
        /// <param name="data">The data.</param>
        public static ExceptionFactoryResult ThrowExceptionIf<TException>(bool condition, string message = null, params KeyValuePair<string, string>[] data) where TException : Exception, new()
        {
            return ThrowExceptionIf<TException>(
                condition,
                () => (TException)Activator.CreateInstance(typeof(TException), message),
                data);
        }

        /// <summary>
        /// Throws the given exception from the delegate if condition is true.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <param name="exception">The exception.</param>
        /// <param name="character">The character.</param>
        /// <param name="data">The data.</param>
        public static ExceptionFactoryResult ThrowExceptionIf<TException>(bool condition, Func<TException> exception, params KeyValuePair<string, string>[] data) where TException : Exception, new()
        {
            if (!condition)
            {
                return new ExceptionFactoryResult();
            }

            TException exceptionToThrow = exception();
            AddExceptionData(exceptionToThrow, data);

            AddExceptionData(
                exceptionToThrow,
                new KeyValuePair<string, string>("Date", DateTime.Now.ToString()));

            throw exceptionToThrow;
        }

        /// <summary>
        /// Adds data to a given exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="character">The character.</param>
        /// <param name="data">The data.</param>
        public static void AddExceptionData(Exception exception, params KeyValuePair<string, string>[] data)
        {
            foreach (var exceptionData in data)
            {
                exception.Data.Add(exceptionData.Key, exceptionData.Value);
            }
        }
    }
}
