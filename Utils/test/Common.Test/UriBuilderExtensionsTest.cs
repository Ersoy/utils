using System;
using NUnit.Framework;
using Utils.Common;

namespace Common.Test {

    public class UriBuilderExtensionsTest {

        [Test]
        public void AddsNewParameter() {
            // Arrange
            const string URL = "http://root.com?q=root";
            var ub = new UriBuilder(URL);

            // Act
            ub.SetQueryStringParameter("key", "value");

            // Assert
            Assert.AreEqual("http://root.com/?q=root&key=value", ub.Uri.ToString());
        }

        [Test]
        public void OverridesExistingParameter() {
            // Arrange
            const string URL = "http://root.com/?key=val";
            var ub = new UriBuilder(URL);

            // Act
            ub.SetQueryStringParameter("key", "value");

            // Assert
            Assert.AreEqual("http://root.com/?key=value", ub.Uri.ToString());
        }
    }

}
