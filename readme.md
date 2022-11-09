# Topelab Core Resolver

Unified layer resolver to different DI services.

At the moment, only Microsoft.Extensions.DependencyInjection, UnityContainer and Autofac are unified with this layer.

## Changes

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

