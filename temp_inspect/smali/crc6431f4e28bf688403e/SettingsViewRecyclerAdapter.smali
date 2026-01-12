.class public Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;
.super Landroidx/recyclerview/widget/RecyclerView$Adapter;
.source "SettingsViewRecyclerAdapter.java"

# interfaces
.implements Lmono/android/IGCUserPeer;
.implements Landroid/view/View$OnClickListener;
.implements Landroid/view/View$OnLongClickListener;


# static fields
.field public static final __md_methods:Ljava/lang/String;


# instance fields
.field private refList:Ljava/util/ArrayList;


# direct methods
.method static constructor <clinit>()V
    .locals 3

    .line 14
    const-string v0, "n_getItemCount:()I:GetGetItemCountHandler\nn_getItemId:(I)J:GetGetItemId_IHandler\nn_getItemViewType:(I)I:GetGetItemViewType_IHandler\nn_onCreateViewHolder:(Landroid/view/ViewGroup;I)Landroidx/recyclerview/widget/RecyclerView$ViewHolder;:GetOnCreateViewHolder_Landroid_view_ViewGroup_IHandler\nn_onBindViewHolder:(Landroidx/recyclerview/widget/RecyclerView$ViewHolder;I)V:GetOnBindViewHolder_Landroidx_recyclerview_widget_RecyclerView_ViewHolder_IHandler\nn_onClick:(Landroid/view/View;)V:GetOnClick_Landroid_view_View_Handler:Android.Views.View/IOnClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onLongClick:(Landroid/view/View;)Z:GetOnLongClick_Landroid_view_View_Handler:Android.Views.View/IOnLongClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n"

    sput-object v0, Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;->__md_methods:Ljava/lang/String;

    .line 23
    const-class v1, Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;

    const-string v2, "AiForms.Renderers.Droid.SettingsViewRecyclerAdapter, SettingsView"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 24
    return-void
.end method

.method public constructor <init>()V
    .locals 3

    .line 29
    invoke-direct {p0}, Landroidx/recyclerview/widget/RecyclerView$Adapter;-><init>()V

    .line 30
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;

    if-ne v0, v1, :cond_0

    .line 31
    const/4 v0, 0x0

    new-array v0, v0, [Ljava/lang/Object;

    const-string v1, "AiForms.Renderers.Droid.SettingsViewRecyclerAdapter, SettingsView"

    const-string v2, ""

    invoke-static {v1, v2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 33
    :cond_0
    return-void
.end method

.method private native n_getItemCount()I
.end method

.method private native n_getItemId(I)J
.end method

.method private native n_getItemViewType(I)I
.end method

.method private native n_onBindViewHolder(Landroidx/recyclerview/widget/RecyclerView$ViewHolder;I)V
.end method

.method private native n_onClick(Landroid/view/View;)V
.end method

.method private native n_onCreateViewHolder(Landroid/view/ViewGroup;I)Landroidx/recyclerview/widget/RecyclerView$ViewHolder;
.end method

.method private native n_onLongClick(Landroid/view/View;)Z
.end method


# virtual methods
.method public getItemCount()I
    .locals 1

    .line 38
    invoke-direct {p0}, Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;->n_getItemCount()I

    move-result v0

    return v0
.end method

.method public getItemId(I)J
    .locals 2

    .line 46
    invoke-direct {p0, p1}, Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;->n_getItemId(I)J

    move-result-wide v0

    return-wide v0
.end method

.method public getItemViewType(I)I
    .locals 0

    .line 54
    invoke-direct {p0, p1}, Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;->n_getItemViewType(I)I

    move-result p1

    return p1
.end method

.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 94
    iget-object v0, p0, Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 95
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;->refList:Ljava/util/ArrayList;

    .line 96
    :cond_0
    iget-object v0, p0, Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 97
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 101
    iget-object v0, p0, Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 102
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 103
    :cond_0
    return-void
.end method

.method public onBindViewHolder(Landroidx/recyclerview/widget/RecyclerView$ViewHolder;I)V
    .locals 0

    .line 70
    invoke-direct {p0, p1, p2}, Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;->n_onBindViewHolder(Landroidx/recyclerview/widget/RecyclerView$ViewHolder;I)V

    .line 71
    return-void
.end method

.method public onClick(Landroid/view/View;)V
    .locals 0

    .line 78
    invoke-direct {p0, p1}, Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;->n_onClick(Landroid/view/View;)V

    .line 79
    return-void
.end method

.method public onCreateViewHolder(Landroid/view/ViewGroup;I)Landroidx/recyclerview/widget/RecyclerView$ViewHolder;
    .locals 0

    .line 62
    invoke-direct {p0, p1, p2}, Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;->n_onCreateViewHolder(Landroid/view/ViewGroup;I)Landroidx/recyclerview/widget/RecyclerView$ViewHolder;

    move-result-object p1

    return-object p1
.end method

.method public onLongClick(Landroid/view/View;)Z
    .locals 0

    .line 86
    invoke-direct {p0, p1}, Lcrc6431f4e28bf688403e/SettingsViewRecyclerAdapter;->n_onLongClick(Landroid/view/View;)Z

    move-result p1

    return p1
.end method
