#pragma checksum "F:\StepByStep\NetCoreGrowthGuide\ToolStep\IdentityServer4Step\IdentityServerStep2\MVCClientC\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "bacd42e26260e6782dc1f708270fe868ef65f774"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "F:\StepByStep\NetCoreGrowthGuide\ToolStep\IdentityServer4Step\IdentityServerStep2\MVCClientC\Views\_ViewImports.cshtml"
using MVCClientC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\StepByStep\NetCoreGrowthGuide\ToolStep\IdentityServer4Step\IdentityServerStep2\MVCClientC\Views\_ViewImports.cshtml"
using MVCClientC.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "F:\StepByStep\NetCoreGrowthGuide\ToolStep\IdentityServer4Step\IdentityServerStep2\MVCClientC\Views\Home\Index.cshtml"
using Microsoft.AspNetCore.Authentication;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bacd42e26260e6782dc1f708270fe868ef65f774", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"caadcf4b54d757771302b62874ff3b0ea0fc5e6f", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<h2>Claims</h2>\r\n\r\n<dl>\r\n");
#nullable restore
#line 6 "F:\StepByStep\NetCoreGrowthGuide\ToolStep\IdentityServer4Step\IdentityServerStep2\MVCClientC\Views\Home\Index.cshtml"
     foreach (var claim in User.Claims)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <dt>");
#nullable restore
#line 8 "F:\StepByStep\NetCoreGrowthGuide\ToolStep\IdentityServer4Step\IdentityServerStep2\MVCClientC\Views\Home\Index.cshtml"
       Write(claim.Type);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dt>\r\n        <dd>");
#nullable restore
#line 9 "F:\StepByStep\NetCoreGrowthGuide\ToolStep\IdentityServer4Step\IdentityServerStep2\MVCClientC\Views\Home\Index.cshtml"
       Write(claim.Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dd>\r\n");
#nullable restore
#line 10 "F:\StepByStep\NetCoreGrowthGuide\ToolStep\IdentityServer4Step\IdentityServerStep2\MVCClientC\Views\Home\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</dl>\r\n\r\n<h2>Properties</h2>\r\n\r\n<dl>\r\n");
#nullable restore
#line 16 "F:\StepByStep\NetCoreGrowthGuide\ToolStep\IdentityServer4Step\IdentityServerStep2\MVCClientC\Views\Home\Index.cshtml"
     foreach (var prop in (await Context.AuthenticateAsync()).Properties.Items)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <dt>");
#nullable restore
#line 18 "F:\StepByStep\NetCoreGrowthGuide\ToolStep\IdentityServer4Step\IdentityServerStep2\MVCClientC\Views\Home\Index.cshtml"
       Write(prop.Key);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dt>\r\n        <dd>");
#nullable restore
#line 19 "F:\StepByStep\NetCoreGrowthGuide\ToolStep\IdentityServer4Step\IdentityServerStep2\MVCClientC\Views\Home\Index.cshtml"
       Write(prop.Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dd>\r\n");
#nullable restore
#line 20 "F:\StepByStep\NetCoreGrowthGuide\ToolStep\IdentityServer4Step\IdentityServerStep2\MVCClientC\Views\Home\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</dl>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
