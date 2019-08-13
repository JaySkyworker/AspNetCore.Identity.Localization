# AspNetCore.Identity.Localization

[![Build Status](https://skyworkertech.visualstudio.com/jayskyworker/_apis/build/status/JaySkyworker.AspNetCore.Identity.Localization?branchName=master)](https://skyworkertech.visualstudio.com/jayskyworker/_build/latest?definitionId=1&branchName=master)

It is quick and simple to create an english web app with Identity 
using Asp.Net Core Mvc 2.2 .
However, if you want to develop a non-english app, you will find many 
obstacles prevent you to have the same experiences.

With many project demonstrate how to localized it, we need a more complete
solution for most of the issues, especially for error messages.

Enjoy!

> **NOTE**: Not all UI text are localized!
> 
> Some less essential pages are not localized, You should use the same method to localize it.

## General Localization

Please have basic knowledge of Localization

https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2

## Basic setup for Localization

See this [commit](https://github.com/JaySkyworker/AspNetCore.Identity.Localization/commit/a4bd2312f0a14090b6f74d667888822cf32dd93b).

- In `Starup.cs`
  - Added `RequestLocalizationOptions` and `supportedCultures`.
  - Added `app.UseRequestLocalization()`
- Added `_SelectLanguagePartial.cshtml` and `SetLanguageController.cs` for a simple UI
  Language selector.


## View / Page / Controller Localization

There are many ways to Localized these general text, I choose to use original english text as Key 
to the resources for quick english fall back.

See this [commit](https://github.com/JaySkyworker/AspNetCore.Identity.Localization/commit/989fce76be2bcf71862976c5a2b3b2459e533af9)
 and this [commit](https://github.com/JaySkyworker/AspNetCore.Identity.Localization/commit/ba281e355c426634b022f050c0b593618874ffb2)
.

Ref:

* https://damienbod.com/2018/07/03/adding-localization-to-the-asp-net-core-identity-pages/

* https://github.com/damienbod/AspNetCorePagesWebpack

## Model / DataAnnotation Localization

For Models and DataAnnotations with assigned text, Ex:
```csharp
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
```
Use `DataAnnotationsLocalization` to localize it, 
However this does NOT localize default display name and default ErrorMessages.
I will cover it in next section.

See this [commit](https://github.com/JaySkyworker/AspNetCore.Identity.Localization/commit/7c706dab8494c118f0a4c8e7d7522ba705a7467f).

- In `Starup.cs`
  - Added `.AddDataAnnotationsLocalization()` after `AddMvc()`.
- Added `DataAnnotationSharedResource.resx`.

### ConventionalMetadataProviders

```csharp
    [Required]
    [EmailAddress]
    public string Email { get; set; }
```
Default Display name for `Email` and 
Default ErrorMessages for `[Required]`, `[EmailAddress]` and etc. 
can be localized by `.ModelMetadataDetailsProviders`

See this [commit](https://github.com/JaySkyworker/AspNetCore.Identity.Localization/commit/326b72d08d83fd7ef8b52795c63a55cf5d5eaa22)
and this [commit](https://github.com/JaySkyworker/AspNetCore.Identity.Localization/commit/20d2b24716c3eb194d9de68484d939e27bf5c2bc)
.

- In `Starup.cs`
  - Added `ConventionalDisplayMetadataProvider`, `ConventionalValidationMetadataProvider`  and related classes.
  - In Mvc Options, Added `options.SetConventionalMetadataProviders()` to Init.
- Added `ValidationMetadataSharedResource.resx` for Validation ErrorMessages.
- Added `DisplayMetadataSharedResource.resx` for Display name.

Ref:
* https://github.com/ridercz/Altairis.ConventionalMetadataProviders

#### ConventionalDisplayMetadataProvider

For Display name, the provider is based on conventions and tries to 
find resource key based on the following naming scheme:

> [[Namespace_]TypeName_]PropertyName

The provider will search resource key from more specific to less, 
stopping on the first where it succeeds:

- `App_Areas_Identity_Pages_Account_LoginModel_InputModel_Email`
- `Areas_Identity_Pages_Account_LoginModel_InputModel_Email`
- `Identity_Pages_Account_LoginModel_InputModel_Email`
- ...
- `LoginModel_InputModel_Email`
- `InputModel_Email`
- `Email`

#### ConventionalValidationMetadataProvider

For Default Validation ErrorMessages, same rule applied:

> [[[Namespace_]TypeName_]PropertyName_]ValidatorType

The provider will search resource key from more specific to less, 
stopping on the first where it succeeds:

- `App_Areas_Identity_Pages_Account_LoginModel_InputModel_Email_Required`
- `Areas_Identity_Pages_Account_LoginModel_InputModel_Email_Required`
- `Identity_Pages_Account_LoginModel_InputModel_Email_Required`
- ...
- `InputModel_Email_Required`
- `Email_Required`
- `Required`

You should keep one message for each validation type for Default ErrorMessages.
See [DefaultValidationMessages.resx](https://github.com/JaySkyworker/AspNetCore.Identity.Localization/blob/feature/ConventionalMetadataProviders/Resources/DefaultValidationMessages.resx)

## Identity Validation Error Localization

Identity itself validates Password settings, etc. And generates some ErrorMessages that needs 
to be localized.

See this [commit](https://github.com/JaySkyworker/AspNetCore.Identity.Localization/commit/bae9cab2ac5684aa720de15adf379c8e7f4b46cc).

- Added `LocalizedIdentityErrorDescriber.cs` and `LocalizedIdentityErrorMessages.resx`.
- In `Starup.cs`
  - Added `.AddErrorDescriber<LocalizedIdentityErrorDescriber>()` next to `AddDefaultIdentity<IdentityUser>()`.

Ref:

* http://www.ziyad.info/en/articles/20-Localizing_Identity_Error_Messages

* https://stackoverflow.com/questions/19961648/how-to-localize-asp-net-identity-username-and-password-error-messages


## How to contribute a translation
Language contributions are welcome!

**How to submit a new translation:**

1. Fork the repo.
1. Create resource files for the language you want to translate.
1. Translate it (obviously).
1. Add the new language to the `supportedCultures` list before `zu-ZA` in `Startup.cs`.
1. Open a pull request.

I will do my best to integrate PR as fast as possible.
