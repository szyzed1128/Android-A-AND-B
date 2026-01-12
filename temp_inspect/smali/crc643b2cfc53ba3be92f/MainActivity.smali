.class public Lcrc643b2cfc53ba3be92f/MainActivity;
.super Lcrc643f46942d9dd1fff9/FormsAppCompatActivity;
.source "MainActivity.java"

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
    const-string v0, "n_onNewIntent:(Landroid/content/Intent;)V:GetOnNewIntent_Landroid_content_Intent_Handler\nn_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\nn_onResume:()V:GetOnResumeHandler\nn_onPostResume:()V:GetOnPostResumeHandler\nn_onRequestPermissionsResult:(I[Ljava/lang/String;[I)V:GetOnRequestPermissionsResult_IarrayLjava_lang_String_arrayIHandler\nn_onActivityResult:(IILandroid/content/Intent;)V:GetOnActivityResult_IILandroid_content_Intent_Handler\nn_onConfigurationChanged:(Landroid/content/res/Configuration;)V:GetOnConfigurationChanged_Landroid_content_res_Configuration_Handler\nn_onDestroy:()V:GetOnDestroyHandler\n"

    sput-object v0, Lcrc643b2cfc53ba3be92f/MainActivity;->__md_methods:Ljava/lang/String;

    .line 22
    const-class v1, Lcrc643b2cfc53ba3be92f/MainActivity;

    const-string v2, "CarScannerXamarinForms.Droid.MainActivity, CarDemo.Android"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 23
    return-void
.end method

.method public constructor <init>()V
    .locals 3

    .line 28
    invoke-direct {p0}, Lcrc643f46942d9dd1fff9/FormsAppCompatActivity;-><init>()V

    .line 29
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc643b2cfc53ba3be92f/MainActivity;

    if-ne v0, v1, :cond_0

    .line 30
    const/4 v0, 0x0

    new-array v0, v0, [Ljava/lang/Object;

    const-string v1, "CarScannerXamarinForms.Droid.MainActivity, CarDemo.Android"

    const-string v2, ""

    invoke-static {v1, v2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 32
    :cond_0
    return-void
.end method

.method public constructor <init>(I)V
    .locals 2

    .line 37
    invoke-direct {p0, p1}, Lcrc643f46942d9dd1fff9/FormsAppCompatActivity;-><init>(I)V

    .line 38
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc643b2cfc53ba3be92f/MainActivity;

    if-ne v0, v1, :cond_0

    .line 39
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    invoke-static {p1}, Ljava/lang/Integer;->valueOf(I)Ljava/lang/Integer;

    move-result-object p1

    aput-object p1, v0, v1

    const-string p1, "CarScannerXamarinForms.Droid.MainActivity, CarDemo.Android"

    const-string v1, "System.Int32, mscorlib"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 41
    :cond_0
    return-void
.end method

.method private native n_onActivityResult(IILandroid/content/Intent;)V
.end method

.method private native n_onConfigurationChanged(Landroid/content/res/Configuration;)V
.end method

.method private native n_onCreate(Landroid/os/Bundle;)V
.end method

.method private native n_onDestroy()V
.end method

.method private native n_onNewIntent(Landroid/content/Intent;)V
.end method

.method private native n_onPostResume()V
.end method

.method private native n_onRequestPermissionsResult(I[Ljava/lang/String;[I)V
.end method

.method private native n_onResume()V
.end method


# virtual methods
.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 110
    iget-object v0, p0, Lcrc643b2cfc53ba3be92f/MainActivity;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 111
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc643b2cfc53ba3be92f/MainActivity;->refList:Ljava/util/ArrayList;

    .line 112
    :cond_0
    iget-object v0, p0, Lcrc643b2cfc53ba3be92f/MainActivity;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 113
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 117
    iget-object v0, p0, Lcrc643b2cfc53ba3be92f/MainActivity;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 118
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 119
    :cond_0
    return-void
.end method

.method public onActivityResult(IILandroid/content/Intent;)V
    .locals 0

    .line 86
    invoke-direct {p0, p1, p2, p3}, Lcrc643b2cfc53ba3be92f/MainActivity;->n_onActivityResult(IILandroid/content/Intent;)V

    .line 87
    return-void
.end method

.method public onConfigurationChanged(Landroid/content/res/Configuration;)V
    .locals 0

    .line 94
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainActivity;->n_onConfigurationChanged(Landroid/content/res/Configuration;)V

    .line 95
    return-void
.end method

.method public onCreate(Landroid/os/Bundle;)V
    .locals 0

    .line 54
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainActivity;->n_onCreate(Landroid/os/Bundle;)V

    .line 55
    return-void
.end method

.method public onDestroy()V
    .locals 0

    .line 102
    invoke-direct {p0}, Lcrc643b2cfc53ba3be92f/MainActivity;->n_onDestroy()V

    .line 103
    return-void
.end method

.method public onNewIntent(Landroid/content/Intent;)V
    .locals 0

    .line 46
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainActivity;->n_onNewIntent(Landroid/content/Intent;)V

    .line 47
    return-void
.end method

.method public onPostResume()V
    .locals 0

    .line 70
    invoke-direct {p0}, Lcrc643b2cfc53ba3be92f/MainActivity;->n_onPostResume()V

    .line 71
    return-void
.end method

.method public onRequestPermissionsResult(I[Ljava/lang/String;[I)V
    .locals 0

    .line 78
    invoke-direct {p0, p1, p2, p3}, Lcrc643b2cfc53ba3be92f/MainActivity;->n_onRequestPermissionsResult(I[Ljava/lang/String;[I)V

    .line 79
    return-void
.end method

.method public onResume()V
    .locals 0

    .line 62
    invoke-direct {p0}, Lcrc643b2cfc53ba3be92f/MainActivity;->n_onResume()V

    .line 63
    return-void
.end method
