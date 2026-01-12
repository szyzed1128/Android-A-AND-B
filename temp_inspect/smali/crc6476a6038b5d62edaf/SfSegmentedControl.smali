.class public Lcrc6476a6038b5d62edaf/SfSegmentedControl;
.super Landroid/widget/LinearLayout;
.source "SfSegmentedControl.java"

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
    const-string v0, "n_onLayout:(ZIIII)V:GetOnLayout_ZIIIIHandler\nn_onMeasure:(II)V:GetOnMeasure_IIHandler\nn_onDraw:(Landroid/graphics/Canvas;)V:GetOnDraw_Landroid_graphics_Canvas_Handler\nn_getLayoutDirection:()I:GetGetLayoutDirectionHandler\nn_setLayoutDirection:(I)V:GetSetLayoutDirection_IHandler\n"

    sput-object v0, Lcrc6476a6038b5d62edaf/SfSegmentedControl;->__md_methods:Ljava/lang/String;

    .line 19
    const-class v1, Lcrc6476a6038b5d62edaf/SfSegmentedControl;

    const-string v2, "Syncfusion.Android.Buttons.SfSegmentedControl, Syncfusion.Buttons.XForms.Android"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 20
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;)V
    .locals 2

    .line 25
    invoke-direct {p0, p1}, Landroid/widget/LinearLayout;-><init>(Landroid/content/Context;)V

    .line 26
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc6476a6038b5d62edaf/SfSegmentedControl;

    if-ne v0, v1, :cond_0

    .line 27
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const-string p1, "Syncfusion.Android.Buttons.SfSegmentedControl, Syncfusion.Buttons.XForms.Android"

    const-string v1, "Android.Content.Context, Mono.Android"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 29
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;Landroid/util/AttributeSet;)V
    .locals 2

    .line 34
    invoke-direct {p0, p1, p2}, Landroid/widget/LinearLayout;-><init>(Landroid/content/Context;Landroid/util/AttributeSet;)V

    .line 35
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc6476a6038b5d62edaf/SfSegmentedControl;

    if-ne v0, v1, :cond_0

    .line 36
    const/4 v0, 0x2

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const/4 p1, 0x1

    aput-object p2, v0, p1

    const-string p1, "Syncfusion.Android.Buttons.SfSegmentedControl, Syncfusion.Buttons.XForms.Android"

    const-string p2, "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 38
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;Landroid/util/AttributeSet;I)V
    .locals 2

    .line 43
    invoke-direct {p0, p1, p2, p3}, Landroid/widget/LinearLayout;-><init>(Landroid/content/Context;Landroid/util/AttributeSet;I)V

    .line 44
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc6476a6038b5d62edaf/SfSegmentedControl;

    if-ne v0, v1, :cond_0

    .line 45
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

    const-string p1, "Syncfusion.Android.Buttons.SfSegmentedControl, Syncfusion.Buttons.XForms.Android"

    const-string p2, "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 47
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;Landroid/util/AttributeSet;II)V
    .locals 2

    .line 52
    invoke-direct {p0, p1, p2, p3, p4}, Landroid/widget/LinearLayout;-><init>(Landroid/content/Context;Landroid/util/AttributeSet;II)V

    .line 53
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc6476a6038b5d62edaf/SfSegmentedControl;

    if-ne v0, v1, :cond_0

    .line 54
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

    const-string p1, "Syncfusion.Android.Buttons.SfSegmentedControl, Syncfusion.Buttons.XForms.Android"

    const-string p2, "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 56
    :cond_0
    return-void
.end method

.method private native n_getLayoutDirection()I
.end method

.method private native n_onDraw(Landroid/graphics/Canvas;)V
.end method

.method private native n_onLayout(ZIIII)V
.end method

.method private native n_onMeasure(II)V
.end method

.method private native n_setLayoutDirection(I)V
.end method


# virtual methods
.method public getLayoutDirection()I
    .locals 1

    .line 85
    invoke-direct {p0}, Lcrc6476a6038b5d62edaf/SfSegmentedControl;->n_getLayoutDirection()I

    move-result v0

    return v0
.end method

.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 101
    iget-object v0, p0, Lcrc6476a6038b5d62edaf/SfSegmentedControl;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 102
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc6476a6038b5d62edaf/SfSegmentedControl;->refList:Ljava/util/ArrayList;

    .line 103
    :cond_0
    iget-object v0, p0, Lcrc6476a6038b5d62edaf/SfSegmentedControl;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 104
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 108
    iget-object v0, p0, Lcrc6476a6038b5d62edaf/SfSegmentedControl;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 109
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 110
    :cond_0
    return-void
.end method

.method public onDraw(Landroid/graphics/Canvas;)V
    .locals 0

    .line 77
    invoke-direct {p0, p1}, Lcrc6476a6038b5d62edaf/SfSegmentedControl;->n_onDraw(Landroid/graphics/Canvas;)V

    .line 78
    return-void
.end method

.method public onLayout(ZIIII)V
    .locals 0

    .line 61
    invoke-direct/range {p0 .. p5}, Lcrc6476a6038b5d62edaf/SfSegmentedControl;->n_onLayout(ZIIII)V

    .line 62
    return-void
.end method

.method public onMeasure(II)V
    .locals 0

    .line 69
    invoke-direct {p0, p1, p2}, Lcrc6476a6038b5d62edaf/SfSegmentedControl;->n_onMeasure(II)V

    .line 70
    return-void
.end method

.method public setLayoutDirection(I)V
    .locals 0

    .line 93
    invoke-direct {p0, p1}, Lcrc6476a6038b5d62edaf/SfSegmentedControl;->n_setLayoutDirection(I)V

    .line 94
    return-void
.end method
