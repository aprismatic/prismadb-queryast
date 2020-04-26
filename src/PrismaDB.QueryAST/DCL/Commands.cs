using PrismaDB.QueryAST.DML;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DCL
{
    public abstract class Command : Query { }

    public class SettingsSaveCommand : Command
    {
        public SettingsSaveCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.SettingsSaveCommandToString(this);

        public override object Clone() => new SettingsSaveCommand();
    }

    public class SettingsLoadCommand : Command
    {
        public SettingsLoadCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.SettingsLoadCommandToString(this);

        public override object Clone() => new SettingsLoadCommand();
    }

    public class OpetreeSaveCommand : Command
    {
        public OpetreeSaveCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.OpetreeSaveCommandToString(this);

        public override object Clone() => new OpetreeSaveCommand();
    }

    public class OpetreeLoadCommand : Command
    {
        public OpetreeLoadCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.OpetreeLoadCommandToString(this);

        public override object Clone() => new OpetreeLoadCommand();
    }

    public class OpetreeInsertCommand : Command
    {
        public List<ConstantContainer> Values;

        public OpetreeInsertCommand()
        {
            Values = new List<ConstantContainer>();
        }
        public OpetreeInsertCommand(List<ConstantContainer> values)
        {
            Values = values;
        }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => Values;

        public override string ToString() => DialectResolver.Dialect.OpetreeInsertCommandToString(this);

        public override object Clone()
        {
            var res = new OpetreeInsertCommand();
            foreach (var value in Values)
                res.Values.Add((ConstantContainer)value.Clone());
            return res;
        }
    }

    public class SchemaLoadCommand : Command
    {
        public SchemaLoadCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.SchemaLoadCommandToString(this);

        public override object Clone() => new SchemaLoadCommand();
    }

    public class KeysExportCommand : Command
    {
        public StringConstant FileUri;

        public KeysExportCommand() { FileUri = new StringConstant(""); }

        public KeysExportCommand(string fileUri) { FileUri = new StringConstant(fileUri); }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.KeysExportCommandToString(this);

        public override object Clone() => new KeysExportCommand(FileUri.strvalue);
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

    public class LicenseRefreshCommand : Command
    {
        public LicenseRefreshCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.LicenseRefreshCommandToString(this);

        public override object Clone() => new LicenseRefreshCommand();
    }

    public class LicenseSetKeyCommand : Command
    {
        public StringConstant LicenseKey;

        public LicenseSetKeyCommand() { LicenseKey = new StringConstant(""); }

        public LicenseSetKeyCommand(string licenseKey) { LicenseKey = new StringConstant(licenseKey); }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.LicenseSetKeyCommandToString(this);

        public override object Clone() => new LicenseSetKeyCommand(LicenseKey.strvalue);
    }

    public class LicenseStatusCommand : Command
    {
        public LicenseStatusCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.LicenseStatusCommandToString(this);

        public override object Clone() => new LicenseStatusCommand();
    }

    public class OpetreeStatusCommand : Command
    {
        public OpetreeStatusCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.OpetreeStatusCommandToString(this);

        public override object Clone() => new OpetreeStatusCommand();
    }
}