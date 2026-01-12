.class public Lcrc64c2ad133154c5379f/JSBridge;
.super Ljava/lang/Object;
.source "JSBridge.java"

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
    const-string v0, "n_callCSharpMethod:(Ljava/lang/String;)V:__export__\nn_setPromiseResult:(JZLjava/lang/String;)V:__export__\nn_connectAsync:(Ljava/lang/String;Ljava/lang/String;)V:__export__\nn_disconnectAsync:()V:__export__\nn_testSensorsPage:()V:__export__\nn_testDTCPage:()V:__export__\nn_testFreezeFramePage:()V:__export__\nn_testSettingsPage:()V:__export__\nn_testECUInfoPage:()V:__export__\nn_getBrands:()Ljava/lang/String;:__export__\nn_getProfiles:(Ljava/lang/String;)Ljava/lang/String;:__export__\nn_applyProfile:(Ljava/lang/String;I)V:__export__\nn_getECUList:()Ljava/lang/String;:__export__\nn_readECUInfoAsync:(Ljava/lang/String;)V:__export__\nn_readDTCAsync:(Ljava/lang/String;)V:__export__\nn_clearDTCAsync:(Ljava/lang/String;)V:__export__\nn_readFreezeFrameAsync:(I)V:__export__\nn_getPIDList:()Ljava/lang/String;:__export__\nn_startReadPIDs:(Ljava/lang/String;)Z:__export__\nn_stopReadPIDs:()V:__export__\nn_getAllSensorsAsync:()V:__export__\nn_startBTScan:()Z:__export__\nn_stopBTScan:()Z:__export__\nn_startBLEScan:()Z:__export__\nn_stopBLEScan:()Z:__export__\n"

    sput-object v0, Lcrc64c2ad133154c5379f/JSBridge;->__md_methods:Ljava/lang/String;

    .line 39
    const-class v1, Lcrc64c2ad133154c5379f/JSBridge;

    const-string v2, "CarDemo.Droid.Renderers.JSBridge, CarDemo.Android"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 40
    return-void
.end method

.method public constructor <init>()V
    .locals 3

    .line 45
    invoke-direct {p0}, Ljava/lang/Object;-><init>()V

    .line 46
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc64c2ad133154c5379f/JSBridge;

    if-ne v0, v1, :cond_0

    .line 47
    const/4 v0, 0x0

    new-array v0, v0, [Ljava/lang/Object;

    const-string v1, "CarDemo.Droid.Renderers.JSBridge, CarDemo.Android"

    const-string v2, ""

    invoke-static {v1, v2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 49
    :cond_0
    return-void
.end method

.method private native n_applyProfile(Ljava/lang/String;I)V
.end method

.method private native n_callCSharpMethod(Ljava/lang/String;)V
.end method

.method private native n_clearDTCAsync(Ljava/lang/String;)V
.end method

.method private native n_connectAsync(Ljava/lang/String;Ljava/lang/String;)V
.end method

.method private native n_disconnectAsync()V
.end method

.method private native n_getAllSensorsAsync()V
.end method

.method private native n_getBrands()Ljava/lang/String;
.end method

.method private native n_getECUList()Ljava/lang/String;
.end method

.method private native n_getPIDList()Ljava/lang/String;
.end method

.method private native n_getProfiles(Ljava/lang/String;)Ljava/lang/String;
.end method

.method private native n_readDTCAsync(Ljava/lang/String;)V
.end method

.method private native n_readECUInfoAsync(Ljava/lang/String;)V
.end method

.method private native n_readFreezeFrameAsync(I)V
.end method

.method private native n_setPromiseResult(JZLjava/lang/String;)V
.end method

.method private native n_startBLEScan()Z
.end method

.method private native n_startBTScan()Z
.end method

.method private native n_startReadPIDs(Ljava/lang/String;)Z
.end method

.method private native n_stopBLEScan()Z
.end method

.method private native n_stopBTScan()Z
.end method

.method private native n_stopReadPIDs()V
.end method

.method private native n_testDTCPage()V
.end method

.method private native n_testECUInfoPage()V
.end method

.method private native n_testFreezeFramePage()V
.end method

.method private native n_testSensorsPage()V
.end method

.method private native n_testSettingsPage()V
.end method


# virtual methods
.method public applyProfile(Ljava/lang/String;I)V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 154
    invoke-direct {p0, p1, p2}, Lcrc64c2ad133154c5379f/JSBridge;->n_applyProfile(Ljava/lang/String;I)V

    .line 155
    return-void
.end method

.method public callCSharpMethod(Ljava/lang/String;)V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 55
    invoke-direct {p0, p1}, Lcrc64c2ad133154c5379f/JSBridge;->n_callCSharpMethod(Ljava/lang/String;)V

    .line 56
    return-void
.end method

.method public clearDTCAsync(Ljava/lang/String;)V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 190
    invoke-direct {p0, p1}, Lcrc64c2ad133154c5379f/JSBridge;->n_clearDTCAsync(Ljava/lang/String;)V

    .line 191
    return-void
.end method

.method public connectAsync(Ljava/lang/String;Ljava/lang/String;)V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 73
    invoke-direct {p0, p1, p2}, Lcrc64c2ad133154c5379f/JSBridge;->n_connectAsync(Ljava/lang/String;Ljava/lang/String;)V

    .line 74
    return-void
.end method

.method public disconnectAsync()V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 82
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_disconnectAsync()V

    .line 83
    return-void
.end method

.method public getAllSensorsAsync()V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 235
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_getAllSensorsAsync()V

    .line 236
    return-void
.end method

.method public getBrands()Ljava/lang/String;
    .locals 1
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 136
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_getBrands()Ljava/lang/String;

    move-result-object v0

    return-object v0
.end method

.method public getECUList()Ljava/lang/String;
    .locals 1
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 163
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_getECUList()Ljava/lang/String;

    move-result-object v0

    return-object v0
.end method

.method public getPIDList()Ljava/lang/String;
    .locals 1
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 208
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_getPIDList()Ljava/lang/String;

    move-result-object v0

    return-object v0
.end method

.method public getProfiles(Ljava/lang/String;)Ljava/lang/String;
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 145
    invoke-direct {p0, p1}, Lcrc64c2ad133154c5379f/JSBridge;->n_getProfiles(Ljava/lang/String;)Ljava/lang/String;

    move-result-object p1

    return-object p1
.end method

.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 279
    iget-object v0, p0, Lcrc64c2ad133154c5379f/JSBridge;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 280
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc64c2ad133154c5379f/JSBridge;->refList:Ljava/util/ArrayList;

    .line 281
    :cond_0
    iget-object v0, p0, Lcrc64c2ad133154c5379f/JSBridge;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 282
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 286
    iget-object v0, p0, Lcrc64c2ad133154c5379f/JSBridge;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 287
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 288
    :cond_0
    return-void
.end method

.method public readDTCAsync(Ljava/lang/String;)V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 181
    invoke-direct {p0, p1}, Lcrc64c2ad133154c5379f/JSBridge;->n_readDTCAsync(Ljava/lang/String;)V

    .line 182
    return-void
.end method

.method public readECUInfoAsync(Ljava/lang/String;)V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 172
    invoke-direct {p0, p1}, Lcrc64c2ad133154c5379f/JSBridge;->n_readECUInfoAsync(Ljava/lang/String;)V

    .line 173
    return-void
.end method

.method public readFreezeFrameAsync(I)V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 199
    invoke-direct {p0, p1}, Lcrc64c2ad133154c5379f/JSBridge;->n_readFreezeFrameAsync(I)V

    .line 200
    return-void
.end method

.method public setPromiseResult(JZLjava/lang/String;)V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 64
    invoke-direct {p0, p1, p2, p3, p4}, Lcrc64c2ad133154c5379f/JSBridge;->n_setPromiseResult(JZLjava/lang/String;)V

    .line 65
    return-void
.end method

.method public startBLEScan()Z
    .locals 1
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 262
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_startBLEScan()Z

    move-result v0

    return v0
.end method

.method public startBTScan()Z
    .locals 1
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 244
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_startBTScan()Z

    move-result v0

    return v0
.end method

.method public startReadPIDs(Ljava/lang/String;)Z
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 217
    invoke-direct {p0, p1}, Lcrc64c2ad133154c5379f/JSBridge;->n_startReadPIDs(Ljava/lang/String;)Z

    move-result p1

    return p1
.end method

.method public stopBLEScan()Z
    .locals 1
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 271
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_stopBLEScan()Z

    move-result v0

    return v0
.end method

.method public stopBTScan()Z
    .locals 1
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 253
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_stopBTScan()Z

    move-result v0

    return v0
.end method

.method public stopReadPIDs()V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 226
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_stopReadPIDs()V

    .line 227
    return-void
.end method

.method public testDTCPage()V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 100
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_testDTCPage()V

    .line 101
    return-void
.end method

.method public testECUInfoPage()V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 127
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_testECUInfoPage()V

    .line 128
    return-void
.end method

.method public testFreezeFramePage()V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 109
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_testFreezeFramePage()V

    .line 110
    return-void
.end method

.method public testSensorsPage()V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 91
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_testSensorsPage()V

    .line 92
    return-void
.end method

.method public testSettingsPage()V
    .locals 0
    .annotation runtime Landroid/webkit/JavascriptInterface;
    .end annotation

    .line 118
    invoke-direct {p0}, Lcrc64c2ad133154c5379f/JSBridge;->n_testSettingsPage()V

    .line 119
    return-void
.end method
