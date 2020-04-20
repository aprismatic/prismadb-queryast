﻿using PrismaDB.QueryAST.DML;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DCL
{
    public abstract class Command : Query { }

    public class SaveSettingsCommand : Command
    {
        public SaveSettingsCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.SaveSettingsCommandToString(this);

        public override object Clone() => new SaveSettingsCommand();
    }

    public class LoadSettingsCommand : Command
    {
        public LoadSettingsCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.LoadSettingsCommandToString(this);

        public override object Clone() => new LoadSettingsCommand();
    }

    public class SaveOpetreeCommand : Command
    {
        public SaveOpetreeCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.SaveOpetreeCommandToString(this);

        public override object Clone() => new SaveOpetreeCommand();
    }

    public class LoadOpetreeCommand : Command
    {
        public LoadOpetreeCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.LoadOpetreeCommandToString(this);

        public override object Clone() => new LoadOpetreeCommand();
    }

    public class InsertOpetreeCommand : Command
    {
        public List<ConstantContainer> Values;

        public InsertOpetreeCommand()
        {
            Values = new List<ConstantContainer>();
        }
        public InsertOpetreeCommand(List<ConstantContainer> values)
        {
            Values = values;
        }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => Values;

        public override string ToString() => DialectResolver.Dialect.InsertOpetreeCommandToString(this);

        public override object Clone()
        {
            var res = new InsertOpetreeCommand();
            foreach (var value in Values)
                res.Values.Add((ConstantContainer)value.Clone());
            return res;
        }
    }

    public class LoadSchemaCommand : Command
    {
        public LoadSchemaCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.LoadSchemaCommandToString(this);

        public override object Clone() => new LoadSchemaCommand();
    }

    public class ExportKeysCommand : Command
    {
        public StringConstant FileUri;

        public ExportKeysCommand() { FileUri = new StringConstant(""); }

        public ExportKeysCommand(string fileUri) { FileUri = new StringConstant(fileUri); }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.ExportKeysCommandToString(this);

        public override object Clone() => new ExportKeysCommand(FileUri.strvalue);
    }

    public class RegisterUserCommand : Command
    {
        public StringConstant UserId;
        public StringConstant Password;

        public RegisterUserCommand()
        {
            UserId = new StringConstant("");
            Password = new StringConstant("");
        }

        public RegisterUserCommand(string userId, string password)
        {
            UserId = new StringConstant(userId);
            Password = new StringConstant(password);
        }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.RegisterUserCommandToString(this);

        public override object Clone() => new RegisterUserCommand(UserId.strvalue, Password.strvalue);
    }

    public class BypassCommand : Command
    {
        public Query Query;

        public BypassCommand() { }

        public BypassCommand(Query query) { Query = query; }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => Query.GetConstants();

        public override string ToString() => DialectResolver.Dialect.BypassCommandToString(this);

        public override object Clone() => new BypassCommand((Query)Query.Clone());
    }

    public class RefreshLicenseCommand : Command
    {
        public RefreshLicenseCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.RefreshLicenseCommandToString(this);

        public override object Clone() => new RefreshLicenseCommand();
    }

    public class SetLicenseKeyCommand : Command
    {
        public StringConstant LicenseKey;

        public SetLicenseKeyCommand() { LicenseKey = new StringConstant(""); }

        public SetLicenseKeyCommand(string licenseKey) { LicenseKey = new StringConstant(licenseKey); }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.SetLicenseKeyCommandToString(this);

        public override object Clone() => new SetLicenseKeyCommand(LicenseKey.strvalue);
    }

    public class CheckLicenseStatusCommand : Command
    {
        public CheckLicenseStatusCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.CheckLicenseStatusCommandToString(this);

        public override object Clone() => new CheckLicenseStatusCommand();
    }
}