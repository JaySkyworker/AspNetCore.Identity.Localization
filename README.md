# AspNetCore.Identity.Localization

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

Ref:

* https://damienbod.com/2018/07/03/adding-localization-to-the-asp-net-core-identity-pages/

* https://github.com/damienbod/AspNetCorePagesWebpack

## Model / DataAnnotation Localization

For Models and DataAnnotations with assigned message, Ex:
```csharp
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
```
Use `DataAnnotationsLocalization` to localize it, However this does NOT localize Default ErrorMessages.
I will cover it in next section.

See this [commit](https://github.com/JaySkyworker/AspNetCore.Identity.Localization/commit/7c706dab8494c118f0a4c8e7d7522ba705a7467f).

- In `Starup.cs`
  - Added `.AddDataAnnotationsLocalization()` after `AddMvc()`.
- Added `DataAnnotationSharedResource.resx`.

### Default Validation Error Localization

```csharp
    [Required]
    [EmailAddress]
    [Display(Name = "LoginEmail")]
    public string Email { get; set; }
```
Default ErrorMessages for `[Required]`, `[EmailAddress]` and etc. 
can only be localized by `.ModelMetadataDetailsProviders`

See this [commit](https://github.com/JaySkyworker/AspNetCore.Identity.Localization/commit/2bb5422c6b8556133e047c34386d7dd886e2375d).

- In `Starup.cs`
  - Added `LocalizedValidationMetadataProvider` and related classes.
  - In Mvc Options, Added `ModelMetadataDetailsProviders.Add()`
- Added `ValidationMetadataSharedResource.resx `.

Ref:
* https://github.com/ForEvolve/ForEvolve.AspNetCore.Localization

### ConventionalMetadataProviders (Not in this Sample)

This might be a better way to localize Model and DataAnnotations but not 
in this sample.
https://github.com/ridercz/Altairis.ConventionalMetadataProviders


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
