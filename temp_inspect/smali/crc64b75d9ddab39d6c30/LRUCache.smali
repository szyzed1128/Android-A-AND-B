.class public Lcrc64b75d9ddab39d6c30/LRUCache;
.super Landroid/util/LruCache;
.source "LRUCache.java"

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
    const-string v0, "n_sizeOf:(Ljava/lang/Object;Ljava/lang/Object;)I:GetSizeOf_Ljava_lang_Object_Ljava_lang_Object_Handler\nn_entryRemoved:(ZLjava/lang/Object;Ljava/lang/Object;Ljava/lang/Object;)V:GetEntryRemoved_ZLjava_lang_Object_Ljava_lang_Object_Ljava_lang_Object_Handler\n"

    sput-object v0, Lcrc64b75d9ddab39d6c30/LRUCache;->__md_methods:Ljava/lang/String;

    .line 16
    const-class v1, Lcrc64b75d9ddab39d6c30/LRUCache;

    const-string v2, "FFImageLoading.Cache.LRUCache, FFImageLoading.Platform"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 17
    return-void
.end method

.method public constructor <init>(I)V
    .locals 2

    .line 22
    invoke-direct {p0, p1}, Landroid/util/LruCache;-><init>(I)V

    .line 23
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc64b75d9ddab39d6c30/LRUCache;

    if-ne v0, v1, :cond_0

    .line 24
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    invoke-static {p1}, Ljava/lang/Integer;->valueOf(I)Ljava/lang/Integer;

    move-result-object p1

    aput-object p1, v0, v1

    const-string p1, "FFImageLoading.Cache.LRUCache, FFImageLoading.Platform"

    const-string v1, "System.Int32, mscorlib"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 26
    :cond_0
    return-void
.end method

.method private native n_entryRemoved(ZLjava/lang/Object;Ljava/lang/Object;Ljava/lang/Object;)V
.end method

.method private native n_sizeOf(Ljava/lang/Object;Ljava/lang/Object;)I
.end method


# virtual methods
.method public entryRemoved(ZLjava/lang/Object;Ljava/lang/Object;Ljava/lang/Object;)V
    .locals 0

    .line 39
    invoke-direct {p0, p1, p2, p3, p4}, Lcrc64b75d9ddab39d6c30/LRUCache;->n_entryRemoved(ZLjava/lang/Object;Ljava/lang/Object;Ljava/lang/Object;)V

    .line 40
    return-void
.end method

.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 47
    iget-object v0, p0, Lcrc64b75d9ddab39d6c30/LRUCache;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 48
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc64b75d9ddab39d6c30/LRUCache;->refList:Ljava/util/ArrayList;

    .line 49
    :cond_0
    iget-object v0, p0, Lcrc64b75d9ddab39d6c30/LRUCache;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 50
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 54
    iget-object v0, p0, Lcrc64b75d9ddab39d6c30/LRUCache;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 55
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 56
    :cond_0
    return-void
.end method

.method public sizeOf(Ljava/lang/Object;Ljava/lang/Object;)I
    .locals 0

    .line 31
    invoke-direct {p0, p1, p2}, Lcrc64b75d9ddab39d6c30/LRUCache;->n_sizeOf(Ljava/lang/Object;Ljava/lang/Object;)I

    move-result p1

    return p1
.end method
