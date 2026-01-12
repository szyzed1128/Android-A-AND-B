.class public final Lcom/google/android/gms/maps/model/MarkerOptions;
.super Lcom/google/android/gms/common/internal/safeparcel/AbstractSafeParcelable;


# static fields
.field public static final CREATOR:Landroid/os/Parcelable$Creator;
    .annotation system Ldalvik/annotation/Signature;
        value = {
            "Landroid/os/Parcelable$Creator<",
            "Lcom/google/android/gms/maps/model/MarkerOptions;",
            ">;"
        }
    .end annotation
.end field


# instance fields
.field private alpha:F

.field private position:Lcom/google/android/gms/maps/model/LatLng;

.field private zzcs:F

.field private zzct:Z

.field private zzdb:F

.field private zzdc:F

.field private zzdn:Ljava/lang/String;

.field private zzdo:Ljava/lang/String;

.field private zzdp:Lcom/google/android/gms/maps/model/BitmapDescriptor;

.field private zzdq:Z

.field private zzdr:Z

.field private zzds:F

.field private zzdt:F

.field private zzdu:F


# direct methods
.method static constructor <clinit>()V
    .locals 1

    .line 131
    new-instance v0, Lcom/google/android/gms/maps/model/zzh;

    invoke-direct {v0}, Lcom/google/android/gms/maps/model/zzh;-><init>()V

    sput-object v0, Lcom/google/android/gms/maps/model/MarkerOptions;->CREATOR:Landroid/os/Parcelable$Creator;

    return-void
.end method

.method public constructor <init>()V
    .locals 3

    .line 1
    invoke-direct {p0}, Lcom/google/android/gms/common/internal/safeparcel/AbstractSafeParcelable;-><init>()V

    .line 2
    const/high16 v0, 0x3f000000    # 0.5f

    iput v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdb:F

    .line 3
    const/high16 v1, 0x3f800000    # 1.0f

    iput v1, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdc:F

    .line 4
    const/4 v2, 0x1

    iput-boolean v2, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzct:Z

    .line 5
    const/4 v2, 0x0

    iput-boolean v2, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdr:Z

    .line 6
    const/4 v2, 0x0

    iput v2, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzds:F

    .line 7
    iput v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdt:F

    .line 8
    iput v2, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdu:F

    .line 9
    iput v1, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->alpha:F

    .line 10
    return-void
.end method

.method constructor <init>(Lcom/google/android/gms/maps/model/LatLng;Ljava/lang/String;Ljava/lang/String;Landroid/os/IBinder;FFZZZFFFFF)V
    .locals 4

    .line 11
    move-object v0, p0

    invoke-direct {p0}, Lcom/google/android/gms/common/internal/safeparcel/AbstractSafeParcelable;-><init>()V

    .line 12
    const/high16 v1, 0x3f000000    # 0.5f

    iput v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdb:F

    .line 13
    const/high16 v2, 0x3f800000    # 1.0f

    iput v2, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdc:F

    .line 14
    const/4 v3, 0x1

    iput-boolean v3, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzct:Z

    .line 15
    const/4 v3, 0x0

    iput-boolean v3, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdr:Z

    .line 16
    const/4 v3, 0x0

    iput v3, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzds:F

    .line 17
    iput v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdt:F

    .line 18
    iput v3, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdu:F

    .line 19
    iput v2, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->alpha:F

    .line 20
    move-object v1, p1

    iput-object v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->position:Lcom/google/android/gms/maps/model/LatLng;

    .line 21
    move-object v1, p2

    iput-object v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdn:Ljava/lang/String;

    .line 22
    move-object v1, p3

    iput-object v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdo:Ljava/lang/String;

    .line 23
    if-nez p4, :cond_0

    .line 24
    const/4 v1, 0x0

    iput-object v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdp:Lcom/google/android/gms/maps/model/BitmapDescriptor;

    goto :goto_0

    .line 25
    :cond_0
    new-instance v1, Lcom/google/android/gms/maps/model/BitmapDescriptor;

    .line 26
    invoke-static {p4}, Lcom/google/android/gms/dynamic/IObjectWrapper$Stub;->asInterface(Landroid/os/IBinder;)Lcom/google/android/gms/dynamic/IObjectWrapper;

    move-result-object v2

    invoke-direct {v1, v2}, Lcom/google/android/gms/maps/model/BitmapDescriptor;-><init>(Lcom/google/android/gms/dynamic/IObjectWrapper;)V

    iput-object v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdp:Lcom/google/android/gms/maps/model/BitmapDescriptor;

    .line 27
    :goto_0
    move v1, p5

    iput v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdb:F

    .line 28
    move v1, p6

    iput v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdc:F

    .line 29
    move v1, p7

    iput-boolean v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdq:Z

    .line 30
    move v1, p8

    iput-boolean v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzct:Z

    .line 31
    move v1, p9

    iput-boolean v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdr:Z

    .line 32
    move v1, p10

    iput v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzds:F

    .line 33
    move v1, p11

    iput v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdt:F

    .line 34
    move/from16 v1, p12

    iput v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdu:F

    .line 35
    move/from16 v1, p13

    iput v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->alpha:F

    .line 36
    move/from16 v1, p14

    iput v1, v0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzcs:F

    .line 37
    return-void
.end method


# virtual methods
.method public final alpha(F)Lcom/google/android/gms/maps/model/MarkerOptions;
    .locals 0

    .line 115
    iput p1, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->alpha:F

    .line 116
    return-object p0
.end method

.method public final anchor(FF)Lcom/google/android/gms/maps/model/MarkerOptions;
    .locals 0

    .line 97
    iput p1, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdb:F

    .line 98
    iput p2, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdc:F

    .line 99
    return-object p0
.end method

.method public final draggable(Z)Lcom/google/android/gms/maps/model/MarkerOptions;
    .locals 0

    .line 107
    iput-boolean p1, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdq:Z

    .line 108
    return-object p0
.end method

.method public final flat(Z)Lcom/google/android/gms/maps/model/MarkerOptions;
    .locals 0

    .line 111
    iput-boolean p1, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdr:Z

    .line 112
    return-object p0
.end method

.method public final getAlpha()F
    .locals 1

    .line 129
    iget v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->alpha:F

    return v0
.end method

.method public final getAnchorU()F
    .locals 1

    .line 121
    iget v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdb:F

    return v0
.end method

.method public final getAnchorV()F
    .locals 1

    .line 122
    iget v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdc:F

    return v0
.end method

.method public final getIcon()Lcom/google/android/gms/maps/model/BitmapDescriptor;
    .locals 1

    .line 120
    iget-object v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdp:Lcom/google/android/gms/maps/model/BitmapDescriptor;

    return-object v0
.end method

.method public final getInfoWindowAnchorU()F
    .locals 1

    .line 127
    iget v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdt:F

    return v0
.end method

.method public final getInfoWindowAnchorV()F
    .locals 1

    .line 128
    iget v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdu:F

    return v0
.end method

.method public final getPosition()Lcom/google/android/gms/maps/model/LatLng;
    .locals 1

    .line 117
    iget-object v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->position:Lcom/google/android/gms/maps/model/LatLng;

    return-object v0
.end method

.method public final getRotation()F
    .locals 1

    .line 126
    iget v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzds:F

    return v0
.end method

.method public final getSnippet()Ljava/lang/String;
    .locals 1

    .line 119
    iget-object v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdo:Ljava/lang/String;

    return-object v0
.end method

.method public final getTitle()Ljava/lang/String;
    .locals 1

    .line 118
    iget-object v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdn:Ljava/lang/String;

    return-object v0
.end method

.method public final getZIndex()F
    .locals 1

    .line 130
    iget v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzcs:F

    return v0
.end method

.method public final icon(Lcom/google/android/gms/maps/model/BitmapDescriptor;)Lcom/google/android/gms/maps/model/MarkerOptions;
    .locals 0

    .line 95
    iput-object p1, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdp:Lcom/google/android/gms/maps/model/BitmapDescriptor;

    .line 96
    return-object p0
.end method

.method public final infoWindowAnchor(FF)Lcom/google/android/gms/maps/model/MarkerOptions;
    .locals 0

    .line 100
    iput p1, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdt:F

    .line 101
    iput p2, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdu:F

    .line 102
    return-object p0
.end method

.method public final isDraggable()Z
    .locals 1

    .line 123
    iget-boolean v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdq:Z

    return v0
.end method

.method public final isFlat()Z
    .locals 1

    .line 125
    iget-boolean v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdr:Z

    return v0
.end method

.method public final isVisible()Z
    .locals 1

    .line 124
    iget-boolean v0, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzct:Z

    return v0
.end method

.method public final position(Lcom/google/android/gms/maps/model/LatLng;)Lcom/google/android/gms/maps/model/MarkerOptions;
    .locals 1

    .line 89
    if-eqz p1, :cond_0

    .line 91
    iput-object p1, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->position:Lcom/google/android/gms/maps/model/LatLng;

    .line 92
    return-object p0

    .line 90
    :cond_0
    new-instance p1, Ljava/lang/IllegalArgumentException;

    const-string v0, "latlng cannot be null - a position is required."

    invoke-direct {p1, v0}, Ljava/lang/IllegalArgumentException;-><init>(Ljava/lang/String;)V

    throw p1
.end method

.method public final rotation(F)Lcom/google/android/gms/maps/model/MarkerOptions;
    .locals 0

    .line 113
    iput p1, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzds:F

    .line 114
    return-object p0
.end method

.method public final snippet(Ljava/lang/String;)Lcom/google/android/gms/maps/model/MarkerOptions;
    .locals 0

    .line 105
    iput-object p1, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdo:Ljava/lang/String;

    .line 106
    return-object p0
.end method

.method public final title(Ljava/lang/String;)Lcom/google/android/gms/maps/model/MarkerOptions;
    .locals 0

    .line 103
    iput-object p1, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdn:Ljava/lang/String;

    .line 104
    return-object p0
.end method

.method public final visible(Z)Lcom/google/android/gms/maps/model/MarkerOptions;
    .locals 0

    .line 109
    iput-boolean p1, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzct:Z

    .line 110
    return-object p0
.end method

.method public final writeToParcel(Landroid/os/Parcel;I)V
    .locals 4

    .line 38
    nop

    .line 39
    invoke-static {p1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->beginObjectHeader(Landroid/os/Parcel;)I

    move-result v0

    .line 40
    nop

    .line 41
    invoke-virtual {p0}, Lcom/google/android/gms/maps/model/MarkerOptions;->getPosition()Lcom/google/android/gms/maps/model/LatLng;

    move-result-object v1

    .line 42
    const/4 v2, 0x2

    const/4 v3, 0x0

    invoke-static {p1, v2, v1, p2, v3}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeParcelable(Landroid/os/Parcel;ILandroid/os/Parcelable;IZ)V

    .line 43
    nop

    .line 44
    invoke-virtual {p0}, Lcom/google/android/gms/maps/model/MarkerOptions;->getTitle()Ljava/lang/String;

    move-result-object p2

    .line 45
    const/4 v1, 0x3

    invoke-static {p1, v1, p2, v3}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeString(Landroid/os/Parcel;ILjava/lang/String;Z)V

    .line 46
    nop

    .line 47
    invoke-virtual {p0}, Lcom/google/android/gms/maps/model/MarkerOptions;->getSnippet()Ljava/lang/String;

    move-result-object p2

    .line 48
    const/4 v1, 0x4

    invoke-static {p1, v1, p2, v3}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeString(Landroid/os/Parcel;ILjava/lang/String;Z)V

    .line 49
    nop

    .line 50
    nop

    .line 51
    iget-object p2, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzdp:Lcom/google/android/gms/maps/model/BitmapDescriptor;

    if-nez p2, :cond_0

    .line 52
    const/4 p2, 0x0

    goto :goto_0

    .line 53
    :cond_0
    invoke-virtual {p2}, Lcom/google/android/gms/maps/model/BitmapDescriptor;->zzb()Lcom/google/android/gms/dynamic/IObjectWrapper;

    move-result-object p2

    .line 54
    invoke-interface {p2}, Lcom/google/android/gms/dynamic/IObjectWrapper;->asBinder()Landroid/os/IBinder;

    move-result-object p2

    .line 55
    :goto_0
    nop

    .line 56
    const/4 v1, 0x5

    invoke-static {p1, v1, p2, v3}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeIBinder(Landroid/os/Parcel;ILandroid/os/IBinder;Z)V

    .line 57
    const/4 p2, 0x6

    .line 58
    invoke-virtual {p0}, Lcom/google/android/gms/maps/model/MarkerOptions;->getAnchorU()F

    move-result v1

    .line 59
    invoke-static {p1, p2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeFloat(Landroid/os/Parcel;IF)V

    .line 60
    const/4 p2, 0x7

    .line 61
    invoke-virtual {p0}, Lcom/google/android/gms/maps/model/MarkerOptions;->getAnchorV()F

    move-result v1

    .line 62
    invoke-static {p1, p2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeFloat(Landroid/os/Parcel;IF)V

    .line 63
    const/16 p2, 0x8

    .line 64
    invoke-virtual {p0}, Lcom/google/android/gms/maps/model/MarkerOptions;->isDraggable()Z

    move-result v1

    .line 65
    invoke-static {p1, p2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeBoolean(Landroid/os/Parcel;IZ)V

    .line 66
    const/16 p2, 0x9

    .line 67
    invoke-virtual {p0}, Lcom/google/android/gms/maps/model/MarkerOptions;->isVisible()Z

    move-result v1

    .line 68
    invoke-static {p1, p2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeBoolean(Landroid/os/Parcel;IZ)V

    .line 69
    const/16 p2, 0xa

    .line 70
    invoke-virtual {p0}, Lcom/google/android/gms/maps/model/MarkerOptions;->isFlat()Z

    move-result v1

    .line 71
    invoke-static {p1, p2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeBoolean(Landroid/os/Parcel;IZ)V

    .line 72
    const/16 p2, 0xb

    .line 73
    invoke-virtual {p0}, Lcom/google/android/gms/maps/model/MarkerOptions;->getRotation()F

    move-result v1

    .line 74
    invoke-static {p1, p2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeFloat(Landroid/os/Parcel;IF)V

    .line 75
    const/16 p2, 0xc

    .line 76
    invoke-virtual {p0}, Lcom/google/android/gms/maps/model/MarkerOptions;->getInfoWindowAnchorU()F

    move-result v1

    .line 77
    invoke-static {p1, p2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeFloat(Landroid/os/Parcel;IF)V

    .line 78
    const/16 p2, 0xd

    .line 79
    invoke-virtual {p0}, Lcom/google/android/gms/maps/model/MarkerOptions;->getInfoWindowAnchorV()F

    move-result v1

    .line 80
    invoke-static {p1, p2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeFloat(Landroid/os/Parcel;IF)V

    .line 81
    const/16 p2, 0xe

    .line 82
    invoke-virtual {p0}, Lcom/google/android/gms/maps/model/MarkerOptions;->getAlpha()F

    move-result v1

    .line 83
    invoke-static {p1, p2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeFloat(Landroid/os/Parcel;IF)V

    .line 84
    const/16 p2, 0xf

    .line 85
    invoke-virtual {p0}, Lcom/google/android/gms/maps/model/MarkerOptions;->getZIndex()F

    move-result v1

    .line 86
    invoke-static {p1, p2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeFloat(Landroid/os/Parcel;IF)V

    .line 87
    invoke-static {p1, v0}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->finishObjectHeader(Landroid/os/Parcel;I)V

    .line 88
    return-void
.end method

.method public final zIndex(F)Lcom/google/android/gms/maps/model/MarkerOptions;
    .locals 0

    .line 93
    iput p1, p0, Lcom/google/android/gms/maps/model/MarkerOptions;->zzcs:F

    .line 94
    return-object p0
.end method
