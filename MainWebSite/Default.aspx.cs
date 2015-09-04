//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//-----------------------------------------------------------------------------

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.IdentityModel.Claims;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        IClaimsPrincipal claimsPrincipal = Page.User as IClaimsPrincipal;
        IClaimsIdentity claimsIdentity = ( IClaimsIdentity )claimsPrincipal.Identity;

        // The code below shows claims found in the IClaimsIdentity.
        // TODO: Change code below to do your processing using claims.

        Table claimsTable = new Table();
        TableRow headerRow = new TableRow();

        TableCell claimTypeCell = new TableCell();
        claimTypeCell.Text = "Claim Type";
        claimTypeCell.BorderStyle = BorderStyle.Solid;

        TableCell claimValueCell = new TableCell();
        claimValueCell.Text = "Claim Value";
        claimValueCell.BorderStyle = BorderStyle.Solid;

        headerRow.Cells.Add( claimTypeCell );
        headerRow.Cells.Add( claimValueCell );
        claimsTable.Rows.Add( headerRow );

        TableRow newRow;
        TableCell newClaimTypeCell, newClaimValueCell;
        foreach ( Claim claim in claimsIdentity.Claims )
        {
            newRow = new TableRow();
            newClaimTypeCell = new TableCell();
            newClaimTypeCell.Text = claim.ClaimType;

            newClaimValueCell = new TableCell();
            newClaimValueCell.Text = claim.Value;

            newRow.Cells.Add(newClaimTypeCell);
            newRow.Cells.Add(newClaimValueCell);

            claimsTable.Rows.Add(newRow);
        }

        this.Controls.Add( claimsTable) ;
    }
}
