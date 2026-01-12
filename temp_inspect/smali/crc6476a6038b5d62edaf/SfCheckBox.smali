.class public Lcrc6476a6038b5d62edaf/SfCheckBox;
.super Landroid/widget/CompoundButton;
.source "SfCheckBox.java"

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
    const-string v0, "n_getPaddingLeft:()I:GetGetPaddingLeftHandler\nn_getPaddingRight:()I:GetGetPaddingRightHandler\nn_isEnabled:()Z:GetIsEnabledHandler\nn_setEnabled:(Z)V:GetSetEnabled_ZHandler\nn_onSaveInstanceState:()Landroid/os/Parcelable;:GetOnSaveInstanceStateHandler\nn_setPadding:(IIII)V:GetSetPadding_IIIIHandler\nn_onRestoreInstanceState:(Landroid/os/Parcelable;)V:GetOnRestoreInstanceState_Landroid_os_Parcelable_Handler\nn_setBackgroundColor:(I)V:GetSetBackgroundColor_IHandler\nn_onCreateDrawableState:(I)[I:GetOnCreateDrawableState_IHandler\nn_onLayout:(ZIIII)V:GetOnLayout_ZIIIIHandler\nn_onDraw:(Landroid/graphics/Canvas;)V:GetOnDraw_Landroid_graphics_Canvas_Handler\n"

    sput-object v0, Lcrc6476a6038b5d62edaf/SfCheckBox;->__md_methods:Ljava/lang/String;

    .line 25
    const-class v1, Lcrc6476a6038b5d62edaf/SfCheckBox;

    const-string v2, "Syncfusion.Android.Buttons.SfCheckBox, Syncfusion.Buttons.XForms.Android"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 26
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;)V
    .locals 2

    .line 31
    invoke-direct {p0, p1}, Landroid/widget/CompoundButton;-><init>(Landroid/content/Context;)V

    .line 32
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc6476a6038b5d62edaf/SfCheckBox;

    if-ne v0, v1, :cond_0

    .line 33
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const-string p1, "Syncfusion.Android.Buttons.SfCheckBox, Syncfusion.Buttons.XForms.Android"

    const-string v1, "Android.Content.Context, Mono.Android"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 35
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;Landroid/util/AttributeSet;)V
    .locals 2

    .line 40
    invoke-direct {p0, p1, p2}, Landroid/widget/CompoundButton;-><init>(Landroid/content/Context;Landroid/util/AttributeSet;)V

    .line 41
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc6476a6038b5d62edaf/SfCheckBox;

    if-ne v0, v1, :cond_0

    .line 42
    const/4 v0, 0x2

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const/4 p1, 0x1

    aput-object p2, v0, p1

    const-string p1, "Syncfusion.Android.Buttons.SfCheckBox, Syncfusion.Buttons.XForms.Android"

    const-string p2, "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 44
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;Landroid/util/AttributeSet;I)V
    .locals 2

    .line 49
    invoke-direct {p0, p1, p2, p3}, Landroid/widget/CompoundButton;-><init>(Landroid/content/Context;Landroid/util/AttributeSet;I)V

    .line 50
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc6476a6038b5d62edaf/SfCheckBox;

    if-ne v0, v1, :cond_0

    .line 51
    const/4 v0, 0x3

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const/4 p1, 0x1

    aput-object p2, v0, p1

    const/4 p1, 0x2

    invoke-static {p3}, Ljava/lang/Integer;->valueOf(I)Ljava/lang/Integer;

    move-result-object p2

    aput-object p2, v0, p1

    const-string p1, "Syncfusion.Android.Buttons.SfCheckBox, Syncfusion.Buttons.XForms.Android"

    const-string p2, "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 53
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;Landroid/util/AttributeSet;II)V
    .locals 2

    .line 58
    invoke-direct {p0, p1, p2, p3, p4}, Landroid/widget/CompoundButton;-><init>(Landroid/content/Context;Landroid/util/AttributeSet;II)V

    .line 59
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc6476a6038b5d62edaf/SfCheckBox;

    if-ne v0, v1, :cond_0

    .line 60
    const/4 v0, 0x4

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const/4 p1, 0x1

    aput-object p2, v0, p1

    const/4 p1, 0x2

    invoke-static {p3}, Ljava/lang/Integer;->valueOf(I)Ljava/lang/Integer;

    move-result-object p2

    aput-object p2, v0, p1

    const/4 p1, 0x3

    invoke-static {p4}, Ljava/lang/Integer;->valueOf(I)Ljava/lang/Integer;

    move-result-object p2

    aput-object p2, v0, p1

    const-string p1, "Syncfusion.Android.Buttons.SfCheckBox, Syncfusion.Buttons.XForms.Android"

    const-string p2, "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 62
    :cond_0
    return-void
.end method

.method private native n_getPaddingLeft()I
.end method

.method private native n_getPaddingRight()I
.end method

.method private native n_isEnabled()Z
.end method

.method private native n_onCreateDrawableState(I)[I
.end method

.method private native n_onDraw(Landroid/graphics/Canvas;)V
.end method

.method private native n_onLayout(ZIIII)V
.end method

.method private native n_onRestoreInstanceState(Landroid/os/Parcelable;)V
.end method

.method private native n_onSaveInstanceState()Landroid/os/Parcelable;
.end method

.method private native n_setBackgroundColor(I)V
.end method

.method private native n_setEnabled(Z)V
.end method

.method private native n_setPadding(IIII)V
.end method


# virtual methods
.method public getPaddingLeft()I
    .locals 1

    .line 67
    invoke-direct {p0}, Lcrc6476a6038b5d62edaf/SfCheckBox;->n_getPaddingLeft()I

    move-result v0

    return v0
.end method

.method public getPaddingRight()I
    .locals 1

    .line 75
    invoke-direct {p0}, Lcrc6476a6038b5d62edaf/SfCheckBox;->n_getPaddingRight()I

    move-result v0

    return v0
.end method

.method public isEnabled()Z
    .locals 1

    .line 83
    invoke-direct {p0}, Lcrc6476a6038b5d62edaf/SfCheckBox;->n_isEnabled()Z

    move-result v0

    return v0
.end method

.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 155
    iget-object v0, p0, Lcrc6476a6038b5d62edaf/SfCheckBox;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 156
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc6476a6038b5d62edaf/SfCheckBox;->refList:Ljava/util/ArrayList;

    .line 157
    :cond_0
    iget-object v0, p0, Lcrc6476a6038b5d62edaf/SfCheckBox;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 158
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 162
    iget-object v0, p0, Lcrc6476a6038b5d62edaf/SfCheckBox;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 163
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 164
    :cond_0
    return-void
.end method

.method public onCreateDrawableState(I)[I
    .locals 0

    .line 131
    invoke-direct {p0, p1}, Lcrc6476a6038b5d62edaf/SfCheckBox;->n_onCreateDrawableState(I)[I

    move-result-object p1

    return-object p1
.end method

.method public onDraw(Landroid/graphics/Canvas;)V
    .locals 0

    .line 147
    invoke-direct {p0, p1}, Lcrc6476a6038b5d62edaf/SfCheckBox;->n_onDraw(Landroid/graphics/Canvas;)V

    .line 148
    return-void
.end method

.method public onLayout(ZIIII)V
    .locals 0

    .line 139
    invoke-direct/range {p0 .. p5}, Lcrc6476a6038b5d62edaf/SfCheckBox;->n_onLayout(ZIIII)V

    .line 140
    return-void
.end method

.method public onRestoreInstanceState(Landroid/os/Parcelable;)V
    .locals 0

    .line 115
    invoke-direct {p0, p1}, Lcrc6476a6038b5d62edaf/SfCheckBox;->n_onRestoreInstanceState(Landroid/os/Parcelable;)V

    .line 116
    return-void
.end method

.method public onSaveInstanceState()Landroid/os/Parcelable;
    .locals 1

    .line 99
    invoke-direct {p0}, Lcrc6476a6038b5d62edaf/SfCheckBox;->n_onSaveInstanceState()Landroid/os/Parcelable;

    move-result-object v0

    return-object v0
.end method

.method public setBackgroundColor(I)V
    .locals 0

    .line 123
    invoke-direct {p0, p1}, Lcrc6476a6038b5d62edaf/SfCheckBox;->n_setBackgroundColor(I)V

    .line 124
    return-void
.end method

.method public setEnabled(Z)V
    .locals 0

    .line 91
    invoke-direct {p0, p1}, Lcrc6476a6038b5d62edaf/SfCheckBox;->n_setEnabled(Z)V

    .line 92
    return-void
.end method

.method public setPadding(IIII)V
    .locals 0

    .line 107
    invoke-direct {p0, p1, p2, p3, p4}, Lcrc6476a6038b5d62edaf/SfCheckBox;->n_setPadding(IIII)V

    .line 108
    return-void
.end method
