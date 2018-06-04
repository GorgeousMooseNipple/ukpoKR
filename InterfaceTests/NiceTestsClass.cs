using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WindowStripControls;
using System.Threading;
using NUnit.Framework;

using WhiteItems = TestStack.White.UIItems;

namespace FuckingInterfaceTests
{
    [TestFixture]
    public class NiceTestsClass
    {
        TestStack.White.Application app;
        Window window;

        Button submitButton, calculateButton, restartButton;
        TextBox moleculeName, fragmentsNum;
        ListBox fragmentsList, pathsList;
        Label moleculeNameLabel;
        CheckBox testCheckBox;

        [SetUp]
        public void InitApplicationForTests()
        {
            app = TestStack.White.Application.Launch(@"..\..\..\Interface\bin\Debug\Interface.exe");
            window = app.GetWindow("MainWindow");
            submitButton =
                window.Get<Button>(SearchCriteria.ByAutomationId("SubmitInputButton"));
            calculateButton =
                window.Get<Button>(SearchCriteria.ByAutomationId("FindPathsButton"));
            restartButton =
                window.Get<Button>(SearchCriteria.ByAutomationId("RestartButton"));
            moleculeName = 
                window.Get<TextBox>(SearchCriteria.ByAutomationId("MoleculeInputTextBox"));
            fragmentsNum =
                window.Get<TextBox>(SearchCriteria.ByAutomationId("FragNumTextBox"));
            moleculeNameLabel =
                window.Get<Label>(SearchCriteria.ByAutomationId("MoleculeNameLabel"));
            fragmentsList =
                window.Get<ListBox>(SearchCriteria.ByAutomationId("FragmentsListBox"));
            pathsList =
                window.Get<ListBox>(SearchCriteria.ByAutomationId("OutputPathsListBox"));
            testCheckBox =
                window.Get<CheckBox>(SearchCriteria.ByAutomationId("TestDataCheckBox"));
        }

        [Test]
        public void AllElementsFoundTest()
        {
            Assert.IsNotNull(submitButton);
            Assert.IsNotNull(calculateButton);
            Assert.IsNotNull(restartButton);
            Assert.IsNotNull(moleculeName);
            Assert.IsNotNull(fragmentsNum);
            Assert.IsNotNull(fragmentsList);
            Assert.IsNotNull(pathsList);
            Assert.IsNotNull(moleculeNameLabel);
        }

        [Test]
        public void SubmitEmptyInputTest()
        {
            submitButton.Click();
            window.WaitWhileBusy();
            var messageBox =
                window.MessageBox("");
            Assert.IsNotNull(messageBox);
            var mbLabel = messageBox.Get<TestStack.White.UIItems.Label>(SearchCriteria.Indexed(0));
            Assert.AreEqual("Поля ввода не заполнены!", mbLabel.Text);
            messageBox.Close();
        }

        [Test]
        public void OutputIsDisabledTest()
        {
            Assert.IsFalse(restartButton.Enabled);
            Assert.IsFalse(calculateButton.Enabled);
            Assert.IsFalse(fragmentsList.Enabled);
            Assert.IsFalse(pathsList.Enabled);
            Assert.IsFalse(moleculeNameLabel.Enabled);
        }

        [Test]
        public void OutputEnabledAfterInputTest()
        {
            moleculeName.Text = "SomeName";
            fragmentsNum.Text = "4";
            submitButton.Click();
            window.WaitWhileBusy();
            Assert.IsTrue(restartButton.Enabled);
            Assert.IsTrue(calculateButton.Enabled);
            Assert.IsTrue(fragmentsList.Enabled);
            Assert.IsTrue(pathsList.Enabled);
            Assert.IsTrue(moleculeNameLabel.Enabled);
        }

        [Test]
        public void FragmentsListBoxFillTest()
        {
            moleculeName.Text = "SomeFakeValue";
            fragmentsNum.Text = "11";
            int fragNumber = int.Parse(fragmentsNum.Text);
            submitButton.Click();
            window.WaitWhileBusy();
            //fragmentsList =
            //    window.Get<WhiteItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId("FragmentsListBox"));
            //Assert.AreEqual(fragNumber, fragmentsList.Items.Count);
            
            Assert.IsTrue(fragmentsList.Items.Count > 0);
        }

        [Test]
        public void PathsListFillTest()
        {
            testCheckBox.Click();
            moleculeName.Text = "SomeFakeValue";
            fragmentsNum.Text = "11";
            submitButton.Click();
            window.WaitWhileBusy();
            calculateButton.Click();
            window.WaitWhileBusy();
            Assert.IsTrue(pathsList.Items.Count > 0);
        }

        [Test]
        public void RestartButtonTest()
        {
            moleculeName.Text = "JustToEnable";
            fragmentsNum.Text = "2";
            submitButton.Click();
            window.WaitWhileBusy();
            restartButton.Click();
            window.WaitWhileBusy();
            Assert.AreEqual(pathsList.Items.Count, 0);
            Assert.AreEqual(fragmentsList.Items.Count, 0);
            Assert.AreEqual(moleculeNameLabel.Text, "");
        }

        [TearDown]
        public void CleanUpAfterTests()
        {
            if(window != null && !window.IsClosed)
                window.Close();
            app.Close();
        }
    }
}
