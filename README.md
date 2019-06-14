# AspNetCore.Identity.Localization

Welcome to Sample Application for Localizing Asp.Net Core MVC 2.2 with Identity pages.

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

See this [commit](https://github.com/JaySkyworker/AspNetCore.Identity.Localization/commit/989fce76be2bcf71862976c5a2b3b2459e533af9)
 and this [commit](https://github.com/JaySkyworker/AspNetCore.Identity.Localization/commit/ba281e355c426634b022f050c0b593618874ffb2)

Ref:

* https://damienbod.com/2018/07/03/adding-localization-to-the-asp-net-core-identity-pages/

* https://github.com/damienbod/AspNetCorePagesWebpack

## Model / DataAnnotation Localization

See this [commit](https://github.com/JaySkyworker/AspNetCore.Identity.Localization/commit/7c706dab8494c118f0a4c8e7d7522ba705a7467f).

### Default Validation Error Localization

See this [commit](https://github.com/JaySkyworker/AspNetCore.Identity.Localization/commit/2bb5422c6b8556133e047c34386d7dd886e2375d).

Ref:
* https://github.com/ForEvolve/ForEvolve.AspNetCore.Localization

### ConventionalMetadataProviders (Not in this Sample)

This might be a better way to localize Model and DataAnnotations but not 
in this sample.
https://github.com/ridercz/Altairis.ConventionalMetadataProviders


## Identity Validation Error Localization

Identity itself validates Password settings and etc. And generates some ErrorMessages that needs 
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

I will do my best to integrates PR as fast as possible.
