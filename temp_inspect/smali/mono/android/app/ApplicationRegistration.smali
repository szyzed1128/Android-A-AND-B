.class public Lmono/android/app/ApplicationRegistration;
.super Ljava/lang/Object;
.source "ApplicationRegistration.java"


# direct methods
.method public constructor <init>()V
    .locals 0

    .line 3
    invoke-direct {p0}, Ljava/lang/Object;-><init>()V

    return-void
.end method

.method public static registerApplications()V
    .locals 3

    .line 8
    const-class v0, Lcrc643b2cfc53ba3be92f/MainApplication;

    sget-object v1, Lcrc643b2cfc53ba3be92f/MainApplication;->__md_methods:Ljava/lang/String;

    const-string v2, "CarScannerXamarinForms.Droid.MainApplication, CarDemo.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"

    invoke-static {v2, v0, v1}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 10
    return-void
.end method
