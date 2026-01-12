.class public final Lcom/google/android/gms/maps/StreetViewPanoramaOptions;
.super Lcom/google/android/gms/common/internal/safeparcel/AbstractSafeParcelable;

# interfaces
.implements Lcom/google/android/gms/common/internal/ReflectedParcelable;


# static fields
.field public static final CREATOR:Landroid/os/Parcelable$Creator;
    .annotation system Ldalvik/annotation/Signature;
        value = {
            "Landroid/os/Parcelable$Creator<",
            "Lcom/google/android/gms/maps/StreetViewPanoramaOptions;",
            ">;"
        }
    .end annotation
.end field


# instance fields
.field private panoId:Ljava/lang/String;

.field private position:Lcom/google/android/gms/maps/model/LatLng;

.field private zzak:Ljava/lang/Boolean;

.field private zzap:Ljava/lang/Boolean;

.field private zzbx:Lcom/google/android/gms/maps/model/StreetViewPanoramaCamera;

.field private zzby:Ljava/lang/Integer;

.field private zzbz:Ljava/lang/Boolean;

.field private zzca:Ljava/lang/Boolean;

.field private zzcb:Ljava/lang/Boolean;

.field private zzcc:Lcom/google/android/gms/maps/model/StreetViewSource;


# direct methods
.method static constructor <clinit>()V
    .locals 1

    .line 116
    new-instance v0, Lcom/google/android/gms/maps/zzai;

    invoke-direct {v0}, Lcom/google/android/gms/maps/zzai;-><init>()V

    sput-object v0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->CREATOR:Landroid/os/Parcelable$Creator;

    return-void
.end method

.method public constructor <init>()V
    .locals 1

    .line 56
    invoke-direct {p0}, Lcom/google/android/gms/common/internal/safeparcel/AbstractSafeParcelable;-><init>()V

    .line 57
    nop

    .line 58
    const/4 v0, 0x1

    invoke-static {v0}, Ljava/lang/Boolean;->valueOf(Z)Ljava/lang/Boolean;

    move-result-object v0

    iput-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzbz:Ljava/lang/Boolean;

    .line 59
    nop

    .line 60
    iput-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzap:Ljava/lang/Boolean;

    .line 61
    nop

    .line 62
    iput-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzca:Ljava/lang/Boolean;

    .line 63
    nop

    .line 64
    iput-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzcb:Ljava/lang/Boolean;

    .line 65
    sget-object v0, Lcom/google/android/gms/maps/model/StreetViewSource;->DEFAULT:Lcom/google/android/gms/maps/model/StreetViewSource;

    iput-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzcc:Lcom/google/android/gms/maps/model/StreetViewSource;

    .line 66
    return-void
.end method

.method constructor <init>(Lcom/google/android/gms/maps/model/StreetViewPanoramaCamera;Ljava/lang/String;Lcom/google/android/gms/maps/model/LatLng;Ljava/lang/Integer;BBBBBLcom/google/android/gms/maps/model/StreetViewSource;)V
    .locals 1

    .line 1
    invoke-direct {p0}, Lcom/google/android/gms/common/internal/safeparcel/AbstractSafeParcelable;-><init>()V

    .line 2
    nop

    .line 3
    const/4 v0, 0x1

    invoke-static {v0}, Ljava/lang/Boolean;->valueOf(Z)Ljava/lang/Boolean;

    move-result-object v0

    iput-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzbz:Ljava/lang/Boolean;

    .line 4
    nop

    .line 5
    iput-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzap:Ljava/lang/Boolean;

    .line 6
    nop

    .line 7
    iput-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzca:Ljava/lang/Boolean;

    .line 8
    nop

    .line 9
    iput-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzcb:Ljava/lang/Boolean;

    .line 10
    sget-object v0, Lcom/google/android/gms/maps/model/StreetViewSource;->DEFAULT:Lcom/google/android/gms/maps/model/StreetViewSource;

    iput-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzcc:Lcom/google/android/gms/maps/model/StreetViewSource;

    .line 11
    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzbx:Lcom/google/android/gms/maps/model/StreetViewPanoramaCamera;

    .line 12
    iput-object p3, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->position:Lcom/google/android/gms/maps/model/LatLng;

    .line 13
    iput-object p4, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzby:Ljava/lang/Integer;

    .line 14
    iput-object p2, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->panoId:Ljava/lang/String;

    .line 15
    invoke-static {p5}, Lcom/google/android/gms/maps/internal/zza;->zza(B)Ljava/lang/Boolean;

    move-result-object p1

    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzbz:Ljava/lang/Boolean;

    .line 16
    invoke-static {p6}, Lcom/google/android/gms/maps/internal/zza;->zza(B)Ljava/lang/Boolean;

    move-result-object p1

    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzap:Ljava/lang/Boolean;

    .line 17
    invoke-static {p7}, Lcom/google/android/gms/maps/internal/zza;->zza(B)Ljava/lang/Boolean;

    move-result-object p1

    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzca:Ljava/lang/Boolean;

    .line 18
    invoke-static {p8}, Lcom/google/android/gms/maps/internal/zza;->zza(B)Ljava/lang/Boolean;

    move-result-object p1

    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzcb:Ljava/lang/Boolean;

    .line 19
    invoke-static {p9}, Lcom/google/android/gms/maps/internal/zza;->zza(B)Ljava/lang/Boolean;

    move-result-object p1

    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzak:Ljava/lang/Boolean;

    .line 20
    iput-object p10, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzcc:Lcom/google/android/gms/maps/model/StreetViewSource;

    .line 21
    return-void
.end method


# virtual methods
.method public final getPanningGesturesEnabled()Ljava/lang/Boolean;
    .locals 1

    .line 100
    iget-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzca:Ljava/lang/Boolean;

    return-object v0
.end method

.method public final getPanoramaId()Ljava/lang/String;
    .locals 1

    .line 97
    iget-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->panoId:Ljava/lang/String;

    return-object v0
.end method

.method public final getPosition()Lcom/google/android/gms/maps/model/LatLng;
    .locals 1

    .line 94
    iget-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->position:Lcom/google/android/gms/maps/model/LatLng;

    return-object v0
.end method

.method public final getRadius()Ljava/lang/Integer;
    .locals 1

    .line 95
    iget-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzby:Ljava/lang/Integer;

    return-object v0
.end method

.method public final getSource()Lcom/google/android/gms/maps/model/StreetViewSource;
    .locals 1

    .line 96
    iget-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzcc:Lcom/google/android/gms/maps/model/StreetViewSource;

    return-object v0
.end method

.method public final getStreetNamesEnabled()Ljava/lang/Boolean;
    .locals 1

    .line 101
    iget-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzcb:Ljava/lang/Boolean;

    return-object v0
.end method

.method public final getStreetViewPanoramaCamera()Lcom/google/android/gms/maps/model/StreetViewPanoramaCamera;
    .locals 1

    .line 93
    iget-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzbx:Lcom/google/android/gms/maps/model/StreetViewPanoramaCamera;

    return-object v0
.end method

.method public final getUseViewLifecycleInFragment()Ljava/lang/Boolean;
    .locals 1

    .line 102
    iget-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzak:Ljava/lang/Boolean;

    return-object v0
.end method

.method public final getUserNavigationEnabled()Ljava/lang/Boolean;
    .locals 1

    .line 98
    iget-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzbz:Ljava/lang/Boolean;

    return-object v0
.end method

.method public final getZoomGesturesEnabled()Ljava/lang/Boolean;
    .locals 1

    .line 99
    iget-object v0, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzap:Ljava/lang/Boolean;

    return-object v0
.end method

.method public final panningGesturesEnabled(Z)Lcom/google/android/gms/maps/StreetViewPanoramaOptions;
    .locals 0

    .line 87
    invoke-static {p1}, Ljava/lang/Boolean;->valueOf(Z)Ljava/lang/Boolean;

    move-result-object p1

    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzca:Ljava/lang/Boolean;

    .line 88
    return-object p0
.end method

.method public final panoramaCamera(Lcom/google/android/gms/maps/model/StreetViewPanoramaCamera;)Lcom/google/android/gms/maps/StreetViewPanoramaOptions;
    .locals 0

    .line 67
    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzbx:Lcom/google/android/gms/maps/model/StreetViewPanoramaCamera;

    .line 68
    return-object p0
.end method

.method public final panoramaId(Ljava/lang/String;)Lcom/google/android/gms/maps/StreetViewPanoramaOptions;
    .locals 0

    .line 69
    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->panoId:Ljava/lang/String;

    .line 70
    return-object p0
.end method

.method public final position(Lcom/google/android/gms/maps/model/LatLng;)Lcom/google/android/gms/maps/StreetViewPanoramaOptions;
    .locals 0

    .line 71
    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->position:Lcom/google/android/gms/maps/model/LatLng;

    .line 72
    return-object p0
.end method

.method public final position(Lcom/google/android/gms/maps/model/LatLng;Lcom/google/android/gms/maps/model/StreetViewSource;)Lcom/google/android/gms/maps/StreetViewPanoramaOptions;
    .locals 0

    .line 80
    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->position:Lcom/google/android/gms/maps/model/LatLng;

    .line 81
    iput-object p2, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzcc:Lcom/google/android/gms/maps/model/StreetViewSource;

    .line 82
    return-object p0
.end method

.method public final position(Lcom/google/android/gms/maps/model/LatLng;Ljava/lang/Integer;)Lcom/google/android/gms/maps/StreetViewPanoramaOptions;
    .locals 0

    .line 73
    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->position:Lcom/google/android/gms/maps/model/LatLng;

    .line 74
    iput-object p2, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzby:Ljava/lang/Integer;

    .line 75
    return-object p0
.end method

.method public final position(Lcom/google/android/gms/maps/model/LatLng;Ljava/lang/Integer;Lcom/google/android/gms/maps/model/StreetViewSource;)Lcom/google/android/gms/maps/StreetViewPanoramaOptions;
    .locals 0

    .line 76
    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->position:Lcom/google/android/gms/maps/model/LatLng;

    .line 77
    iput-object p2, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzby:Ljava/lang/Integer;

    .line 78
    iput-object p3, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzcc:Lcom/google/android/gms/maps/model/StreetViewSource;

    .line 79
    return-object p0
.end method

.method public final streetNamesEnabled(Z)Lcom/google/android/gms/maps/StreetViewPanoramaOptions;
    .locals 0

    .line 89
    invoke-static {p1}, Ljava/lang/Boolean;->valueOf(Z)Ljava/lang/Boolean;

    move-result-object p1

    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzcb:Ljava/lang/Boolean;

    .line 90
    return-object p0
.end method

.method public final toString()Ljava/lang/String;
    .locals 3

    .line 103
    invoke-static {p0}, Lcom/google/android/gms/common/internal/Objects;->toStringHelper(Ljava/lang/Object;)Lcom/google/android/gms/common/internal/Objects$ToStringHelper;

    move-result-object v0

    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->panoId:Ljava/lang/String;

    .line 104
    const-string v2, "PanoramaId"

    invoke-virtual {v0, v2, v1}, Lcom/google/android/gms/common/internal/Objects$ToStringHelper;->add(Ljava/lang/String;Ljava/lang/Object;)Lcom/google/android/gms/common/internal/Objects$ToStringHelper;

    move-result-object v0

    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->position:Lcom/google/android/gms/maps/model/LatLng;

    .line 105
    const-string v2, "Position"

    invoke-virtual {v0, v2, v1}, Lcom/google/android/gms/common/internal/Objects$ToStringHelper;->add(Ljava/lang/String;Ljava/lang/Object;)Lcom/google/android/gms/common/internal/Objects$ToStringHelper;

    move-result-object v0

    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzby:Ljava/lang/Integer;

    .line 106
    const-string v2, "Radius"

    invoke-virtual {v0, v2, v1}, Lcom/google/android/gms/common/internal/Objects$ToStringHelper;->add(Ljava/lang/String;Ljava/lang/Object;)Lcom/google/android/gms/common/internal/Objects$ToStringHelper;

    move-result-object v0

    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzcc:Lcom/google/android/gms/maps/model/StreetViewSource;

    .line 107
    const-string v2, "Source"

    invoke-virtual {v0, v2, v1}, Lcom/google/android/gms/common/internal/Objects$ToStringHelper;->add(Ljava/lang/String;Ljava/lang/Object;)Lcom/google/android/gms/common/internal/Objects$ToStringHelper;

    move-result-object v0

    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzbx:Lcom/google/android/gms/maps/model/StreetViewPanoramaCamera;

    .line 108
    const-string v2, "StreetViewPanoramaCamera"

    invoke-virtual {v0, v2, v1}, Lcom/google/android/gms/common/internal/Objects$ToStringHelper;->add(Ljava/lang/String;Ljava/lang/Object;)Lcom/google/android/gms/common/internal/Objects$ToStringHelper;

    move-result-object v0

    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzbz:Ljava/lang/Boolean;

    .line 109
    const-string v2, "UserNavigationEnabled"

    invoke-virtual {v0, v2, v1}, Lcom/google/android/gms/common/internal/Objects$ToStringHelper;->add(Ljava/lang/String;Ljava/lang/Object;)Lcom/google/android/gms/common/internal/Objects$ToStringHelper;

    move-result-object v0

    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzap:Ljava/lang/Boolean;

    .line 110
    const-string v2, "ZoomGesturesEnabled"

    invoke-virtual {v0, v2, v1}, Lcom/google/android/gms/common/internal/Objects$ToStringHelper;->add(Ljava/lang/String;Ljava/lang/Object;)Lcom/google/android/gms/common/internal/Objects$ToStringHelper;

    move-result-object v0

    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzca:Ljava/lang/Boolean;

    .line 111
    const-string v2, "PanningGesturesEnabled"

    invoke-virtual {v0, v2, v1}, Lcom/google/android/gms/common/internal/Objects$ToStringHelper;->add(Ljava/lang/String;Ljava/lang/Object;)Lcom/google/android/gms/common/internal/Objects$ToStringHelper;

    move-result-object v0

    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzcb:Ljava/lang/Boolean;

    .line 112
    const-string v2, "StreetNamesEnabled"

    invoke-virtual {v0, v2, v1}, Lcom/google/android/gms/common/internal/Objects$ToStringHelper;->add(Ljava/lang/String;Ljava/lang/Object;)Lcom/google/android/gms/common/internal/Objects$ToStringHelper;

    move-result-object v0

    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzak:Ljava/lang/Boolean;

    .line 113
    const-string v2, "UseViewLifecycleInFragment"

    invoke-virtual {v0, v2, v1}, Lcom/google/android/gms/common/internal/Objects$ToStringHelper;->add(Ljava/lang/String;Ljava/lang/Object;)Lcom/google/android/gms/common/internal/Objects$ToStringHelper;

    move-result-object v0

    .line 114
    invoke-virtual {v0}, Lcom/google/android/gms/common/internal/Objects$ToStringHelper;->toString()Ljava/lang/String;

    move-result-object v0

    .line 115
    return-object v0
.end method

.method public final useViewLifecycleInFragment(Z)Lcom/google/android/gms/maps/StreetViewPanoramaOptions;
    .locals 0

    .line 91
    invoke-static {p1}, Ljava/lang/Boolean;->valueOf(Z)Ljava/lang/Boolean;

    move-result-object p1

    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzak:Ljava/lang/Boolean;

    .line 92
    return-object p0
.end method

.method public final userNavigationEnabled(Z)Lcom/google/android/gms/maps/StreetViewPanoramaOptions;
    .locals 0

    .line 83
    invoke-static {p1}, Ljava/lang/Boolean;->valueOf(Z)Ljava/lang/Boolean;

    move-result-object p1

    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzbz:Ljava/lang/Boolean;

    .line 84
    return-object p0
.end method

.method public final writeToParcel(Landroid/os/Parcel;I)V
    .locals 4

    .line 22
    nop

    .line 23
    invoke-static {p1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->beginObjectHeader(Landroid/os/Parcel;)I

    move-result v0

    .line 24
    nop

    .line 25
    invoke-virtual {p0}, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->getStreetViewPanoramaCamera()Lcom/google/android/gms/maps/model/StreetViewPanoramaCamera;

    move-result-object v1

    .line 26
    const/4 v2, 0x2

    const/4 v3, 0x0

    invoke-static {p1, v2, v1, p2, v3}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeParcelable(Landroid/os/Parcel;ILandroid/os/Parcelable;IZ)V

    .line 27
    nop

    .line 28
    invoke-virtual {p0}, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->getPanoramaId()Ljava/lang/String;

    move-result-object v1

    .line 29
    const/4 v2, 0x3

    invoke-static {p1, v2, v1, v3}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeString(Landroid/os/Parcel;ILjava/lang/String;Z)V

    .line 30
    nop

    .line 31
    invoke-virtual {p0}, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->getPosition()Lcom/google/android/gms/maps/model/LatLng;

    move-result-object v1

    .line 32
    const/4 v2, 0x4

    invoke-static {p1, v2, v1, p2, v3}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeParcelable(Landroid/os/Parcel;ILandroid/os/Parcelable;IZ)V

    .line 33
    nop

    .line 34
    invoke-virtual {p0}, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->getRadius()Ljava/lang/Integer;

    move-result-object v1

    .line 35
    const/4 v2, 0x5

    invoke-static {p1, v2, v1, v3}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeIntegerObject(Landroid/os/Parcel;ILjava/lang/Integer;Z)V

    .line 36
    nop

    .line 37
    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzbz:Ljava/lang/Boolean;

    invoke-static {v1}, Lcom/google/android/gms/maps/internal/zza;->zza(Ljava/lang/Boolean;)B

    move-result v1

    .line 38
    const/4 v2, 0x6

    invoke-static {p1, v2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeByte(Landroid/os/Parcel;IB)V

    .line 39
    nop

    .line 40
    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzap:Ljava/lang/Boolean;

    invoke-static {v1}, Lcom/google/android/gms/maps/internal/zza;->zza(Ljava/lang/Boolean;)B

    move-result v1

    .line 41
    const/4 v2, 0x7

    invoke-static {p1, v2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeByte(Landroid/os/Parcel;IB)V

    .line 42
    nop

    .line 43
    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzca:Ljava/lang/Boolean;

    invoke-static {v1}, Lcom/google/android/gms/maps/internal/zza;->zza(Ljava/lang/Boolean;)B

    move-result v1

    .line 44
    const/16 v2, 0x8

    invoke-static {p1, v2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeByte(Landroid/os/Parcel;IB)V

    .line 45
    nop

    .line 46
    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzcb:Ljava/lang/Boolean;

    invoke-static {v1}, Lcom/google/android/gms/maps/internal/zza;->zza(Ljava/lang/Boolean;)B

    move-result v1

    .line 47
    const/16 v2, 0x9

    invoke-static {p1, v2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeByte(Landroid/os/Parcel;IB)V

    .line 48
    nop

    .line 49
    iget-object v1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzak:Ljava/lang/Boolean;

    invoke-static {v1}, Lcom/google/android/gms/maps/internal/zza;->zza(Ljava/lang/Boolean;)B

    move-result v1

    .line 50
    const/16 v2, 0xa

    invoke-static {p1, v2, v1}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeByte(Landroid/os/Parcel;IB)V

    .line 51
    nop

    .line 52
    invoke-virtual {p0}, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->getSource()Lcom/google/android/gms/maps/model/StreetViewSource;

    move-result-object v1

    .line 53
    const/16 v2, 0xb

    invoke-static {p1, v2, v1, p2, v3}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->writeParcelable(Landroid/os/Parcel;ILandroid/os/Parcelable;IZ)V

    .line 54
    invoke-static {p1, v0}, Lcom/google/android/gms/common/internal/safeparcel/SafeParcelWriter;->finishObjectHeader(Landroid/os/Parcel;I)V

    .line 55
    return-void
.end method

.method public final zoomGesturesEnabled(Z)Lcom/google/android/gms/maps/StreetViewPanoramaOptions;
    .locals 0

    .line 85
    invoke-static {p1}, Ljava/lang/Boolean;->valueOf(Z)Ljava/lang/Boolean;

    move-result-object p1

    iput-object p1, p0, Lcom/google/android/gms/maps/StreetViewPanoramaOptions;->zzap:Ljava/lang/Boolean;

    .line 86
    return-object p0
.end method
