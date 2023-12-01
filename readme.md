# Topelab Core Resolver

Unified layer resolver to different DI services.

At the moment, only Microsoft.Extensions.DependencyInjection, UnityContainer and Autofac are unified with this layer.

## Changes

### 1.5.2

- Added `AddLazyTransient<TFrom, TTo>()` to `ResolveInfoCollection` so you can use in constructor `Lazy<TFrom> lazyTypeToResove`

### 1.5.1

- Upgrade to .NET 8.0
- Possibility to use `Resolve<T>()`, `Resolve<T>(params)`, `Resolve<T>(key)` or `Resolve<T>(key, params)` if define `global using static Topelab.Core.Resolver.(Autofact | Microsoft | Unity).ResolverFactory`

### 1.4.15

- Added new methods to `IResolver`:
    - `object Get(Type type)`
    - `object Get(Type typeFrom, string key)`

### 1.4.12

- Upgrade Microsoft.NET.Test.Sdk to 17.6.0

### 1.4.10

- Upgrade Microsoft.Extensions.Hosting to 7.0.1
- Upgrade Microsoft.NET.Test.Sdk to 17.5.0

### 1.4.9

- Unified Resolver.Get<> with parameters. Fixed getting instance with constructor parameters not passed that are resolved on the fly.

    ```cs
    // Register with witch constructor you want to resolve instance
    // IClasTest will resolve instance using constructor ClaseTest(IGeremuDbContext context, string text, int number)

    var resolver = ResolverFactory.Create(new ResolveInfoCollection()
        .AddTransient<IClaseTest, ClaseTest>(typeof(IGeremuDbContext), typeof(string), typeof(int))
        .AddTransient<IGeremuDbContext, GeremuDbContext>()
        );

    // And with this version you can resolve only passing certain parameters

    var claseTest = resolver.Get<IClaseTest, string, int>("hello", 1);
    ```

### 1.4.8

- New method for `ResolveInfoCollection`: `AddInitializer(Action<IResolver> initializer)`, can be used to initialize some extensions with the recently created `IResolver` so it can be used to resolve needed services.

### 1.4.7

- Fix an error on Topelab.Core.Resolver.Microsoft with a trouble in resolving named registration that has to resolve normal registered interfaces
- Upgrade Autofac to 6.5.0
- Upgrade NUnit3TestAdapter to 4.3.1
- Upgrade Microsoft.NET.Test.Sdk to 17.4.1

### 1.4.5

- `ResolverFactory.GetResolver()` will always return first resolver.
- When creating multiples resolvers, any `Get<T>()` will resolve self registered or the last registered on other resolvers. This functionality opens the door to register everywhere on application and resolve using resolver given with `ResolverFactory.GetResolver()`

### 1.4.4

- Upgrade to .NET 7.0

### 1.4.3

- New methods for `ResolveInfoCollection`:
  - `AddTransient(Type typeFrom, Type typeTo, params Type[] constructorParamTypes)`
  - `AddScoped(Type typeFrom, Type typeTo, params Type[] constructorParamTypes)`
  - `AddSingleton(Type typeFrom, Type typeTo, params Type[] constructorParamTypes)`
- Deprecated `Add<TFrom, TTo>(params Type[] constructorParamTypes)`
  - **Use** `AddTransient<TFrom, TTo>(params Type[] constructorParamTypes)`
- Deprecated `Add<TFrom, TTo>(string key, params Type[] constructorParamTypes)`
  - **Use** `AddTransient<TFrom, TTo>(string key, params Type[] constructorParamTypes)`

### 1.4.1

- Upgrade some nugets

### 1.4.0

- Version numbering change

### 1.3.522.529

- Upgrade some nugets

### 1.3.1522.507

- Upgrade some nugets
- Fix AddFactory to support other cycle life time
- Added static *GetResolver* to *ResolverFactory*

### 1.3.1122.121

- Added support for Autofac

### 1.3.1022.121

- Added new methods to `ResolveInfoCollection` to clarify usage:
  - AddSingleton
  - AddScoped
  - AddInstance
  - AddFactory
  - AddCollection
- Split `ResolveTypeEnum` into `ResolveLifeCycleEnum (Transient, Scoped Singleton)` and `ResolveModeEnum (None, Instance, Factory)`
- Set license to MIT License

### 1.3.822.116

- Fix `ResolverFactory.Create` for `Topelab.Core.Resolver.Microsoft`
- Added `Add` to ResolverInfoCollection to add others
- Added possibility to add factories to `ResolveInfoCollection` with new extension `AddFactory`.
- Fix `AddResolver` because of different instance of IServiceProvider was using. 

### 1.3.422.113

- Added `AddResolver` to `Topelab.Core.Resolver.Microsoft` to integrate to build services configuration.

