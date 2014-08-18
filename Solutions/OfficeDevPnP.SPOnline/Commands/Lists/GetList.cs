﻿using OfficeDevPnP.SPOnline.CmdletHelpAttributes;
using OfficeDevPnP.SPOnline.Commands.Base;
using OfficeDevPnP.SPOnline.Commands.Base.PipeBinds;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SPO = OfficeDevPnP.SPOnline.Core;

namespace OfficeDevPnP.SPOnline.Commands
{
    [Cmdlet(VerbsCommon.Get, "SPOList")]
    [CmdletHelp("Returns a List object", DetailedDescription = "Returns a list object. Due to limitation of the PowerShell environment the command does not return a full Client List object. In order to access the full client list, use the ContextObject property of the return lists.")]
    [CmdletExample(Code = "PS:> Get-SPOList", Remarks = "Returns all lists in the current web", SortOrder = 1)]
    [CmdletExample(Code = "PS:> Get-SPOList -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe", Remarks = "Returns a list with the given id.", SortOrder = 2)]
    [CmdletExample(Code = "PS:> Get-SPOList -Identity /Lists/Announcements", Remarks = "Returns a list with the given url.", SortOrder = 3)]
    public class GetList : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID or Url of the list.")]
        public SPOListPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (Identity != null)
            {
                if (Identity.Id != Guid.Empty)
                {
                    WriteObject(new SPOList(SPO.SPOList.GetListById(Identity.Id, SelectedWeb, ClientContext)));
                }
                else if (!string.IsNullOrEmpty(Identity.Title))
                {
                    List list = null;
                    try
                    {
                        list = SPOnline.Core.SPOList.GetListByTitle(Identity.Title, SelectedWeb, ClientContext);
                    }
                    catch
                    {
                        list = SPOnline.Core.SPOList.GetListByUrl(Identity.Title, SelectedWeb, ClientContext);
                    }
                    if (list != null)
                    {
                        WriteObject(new SPOList(list));
                    }
                }
            }
            else
            {
                var query = from list in SPO.SPOList.GetLists(SelectedWeb, ClientContext)
                            select new SPOList(list);
                WriteObject(query.ToList(), true);
            }
        }
    }

}
