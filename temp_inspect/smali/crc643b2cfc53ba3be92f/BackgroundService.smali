.class public Lcrc643b2cfc53ba3be92f/BackgroundService;
.super Landroid/app/Service;
.source "BackgroundService.java"

# interfaces
.implements Lmono/android/IGCUserPeer;


# static fields
.field public static final __md_methods:Ljava/lang/String;


# instance fields
.field private refList:Ljava/util/ArrayList;


# direct methods
.method static constructor <clinit>()V
    .locals 3

    .line 12
    const-string v0, "n_onCreate:()V:GetOnCreateHandler\nn_onBind:(Landroid/content/Intent;)Landroid/os/IBinder;:GetOnBind_Landroid_content_Intent_Handler\nn_onStartCommand:(Landroid/content/Intent;II)I:GetOnStartCommand_Landroid_content_Intent_IIHandler\n"

    sput-object v0, Lcrc643b2cfc53ba3be92f/BackgroundService;->__md_methods:Ljava/lang/String;

    .line 17
    const-class v1, Lcrc643b2cfc53ba3be92f/BackgroundService;

    const-string v2, "CarScannerXamarinForms.Droid.BackgroundService, CarDemo.Android"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 18
    return-void
.end method

.method public constructor <init>()V
    .locals 3

    .line 23
    invoke-direct {p0}, Landroid/app/Service;-><init>()V

    .line 24
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc643b2cfc53ba3be92f/BackgroundService;

    if-ne v0, v1, :cond_0

    .line 25
    const/4 v0, 0x0

    new-array v0, v0, [Ljava/lang/Object;

    const-string v1, "CarScannerXamarinForms.Droid.BackgroundService, CarDemo.Android"

    const-string v2, ""

    invoke-static {v1, v2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 27
    :cond_0
    return-void
.end method

.method private native n_onBind(Landroid/content/Intent;)Landroid/os/IBinder;
.end method

.method private native n_onCreate()V
.end method

.method private native n_onStartCommand(Landroid/content/Intent;II)I
.end method


# virtual methods
.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 56
    iget-object v0, p0, Lcrc643b2cfc53ba3be92f/BackgroundService;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 57
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc643b2cfc53ba3be92f/BackgroundService;->refList:Ljava/util/ArrayList;

    .line 58
    :cond_0
    iget-object v0, p0, Lcrc643b2cfc53ba3be92f/BackgroundService;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 59
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 63
    iget-object v0, p0, Lcrc643b2cfc53ba3be92f/BackgroundService;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 64
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 65
    :cond_0
    return-void
.end method

.method public onBind(Landroid/content/Intent;)Landroid/os/IBinder;
    .locals 0

    .line 40
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/BackgroundService;->n_onBind(Landroid/content/Intent;)Landroid/os/IBinder;

    move-result-object p1

    return-object p1
.end method

.method public onCreate()V
    .locals 0

    .line 32
    invoke-direct {p0}, Lcrc643b2cfc53ba3be92f/BackgroundService;->n_onCreate()V

    .line 33
    return-void
.end method

.method public onStartCommand(Landroid/content/Intent;II)I
    .locals 0

    .line 48
    invoke-direct {p0, p1, p2, p3}, Lcrc643b2cfc53ba3be92f/BackgroundService;->n_onStartCommand(Landroid/content/Intent;II)I

    move-result p1

    return p1
.end method
