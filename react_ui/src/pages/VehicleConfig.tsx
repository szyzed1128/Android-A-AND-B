import React, { useState, useEffect, useMemo, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { NavBar, Search, Cell, CellGroup, Toast, Loading } from 'react-vant';
import { AppContext } from '../hooks/AppContext';

interface Profile {
  Name: string;
  Description?: string;
}

interface BrandData {
  [brandName: string]: Profile[];
}

const VehicleConfig: React.FC = () => {
  const navigate = useNavigate();
  const appContext = useContext(AppContext);

  if (!appContext) {
    throw new Error('VehicleConfig must be used within AppProvider');
  }

  const { setSelectedProfile } = appContext;

  // State
  const [brands, setBrands] = useState<string[]>([]);
  const [selectedBrand, setSelectedBrand] = useState<string | null>(null);
  const [profiles, setProfiles] = useState<Profile[]>([]);
  const [searchKeyword, setSearchKeyword] = useState('');
  const [loading, setLoading] = useState(false);

  // Safe JSBridge call
  const safeCall = (method: string, ...args: any[]) => {
    const bridge = (window as any).JSBridge;
    if (bridge && typeof bridge[method] === 'function') {
      try {
        return bridge[method](...args);
      } catch (e) {
        console.error(`JSBridge.${method} error:`, e);
      }
    } else {
      console.warn(`JSBridge.${method} not available.`);
    }
    return null;
  };

  // Load brands on mount
  useEffect(() => {
    setLoading(true);
    try {
      const result = safeCall('getBrands');
      if (result) {
        const data: string[] = typeof result === 'string' ? JSON.parse(result) : result;
        setBrands(data);
      }
    } catch (e) {
      console.error('Failed to load brands:', e);
      Toast.fail('加载品牌列表失败');
    } finally {
      setLoading(false);
    }
  }, []);

  // Load profiles when brand is selected
  useEffect(() => {
    if (!selectedBrand) {
      setProfiles([]);
      return;
    }

    setLoading(true);
    try {
      const result = safeCall('getProfiles', selectedBrand);
      if (result) {
        const data: Profile[] = typeof result === 'string' ? JSON.parse(result) : result;
        setProfiles(data);
      }
    } catch (e) {
      console.error('Failed to load profiles:', e);
      Toast.fail('加载车型列表失败');
    } finally {
      setLoading(false);
    }
  }, [selectedBrand]);

  // Filtered brands based on search
  const filteredBrands = useMemo(() => {
    if (!searchKeyword.trim()) {
      return brands;
    }
    const keyword = searchKeyword.toLowerCase();
    return brands.filter(brand => brand.toLowerCase().includes(keyword));
  }, [brands, searchKeyword]);

  // Handle brand selection
  const handleBrandSelect = (brand: string) => {
    setSelectedBrand(brand);
    setSearchKeyword('');
  };

  // Handle profile selection
  const handleProfileSelect = (profile: Profile, index: number) => {
    if (!selectedBrand) {
      return;
    }

    try {
      safeCall('applyProfile', selectedBrand, index);
      setSelectedProfile({ brand: selectedBrand, name: profile.Name });
      Toast.success('配置已应用');
      setTimeout(() => {
        navigate('/');
      }, 500);
    } catch (e) {
      console.error('Failed to apply profile:', e);
      Toast.fail('应用配置失败');
    }
  };

  // Handle back navigation
  const handleBack = () => {
    if (selectedBrand) {
      // Go back to brand selection
      setSelectedBrand(null);
      setSearchKeyword('');
    } else {
      // Go back to previous page
      navigate(-1);
    }
  };

  // Render brand list
  const renderBrandList = () => (
    <>
      <Search
        value={searchKeyword}
        onChange={setSearchKeyword}
        placeholder="搜索品牌"
        shape="round"
        style={{ padding: '8px 16px' }}
      />

      {loading ? (
        <div style={{ display: 'flex', justifyContent: 'center', padding: 40 }}>
          <Loading type="spinner" />
        </div>
      ) : filteredBrands.length === 0 ? (
        <div style={{ textAlign: 'center', padding: 40, color: '#999' }}>
          {searchKeyword ? '未找到匹配的品牌' : '暂无品牌数据'}
        </div>
      ) : (
        <CellGroup>
          {filteredBrands.map((brand, index) => (
            <Cell
              key={index}
              title={brand}
              clickable
              isLink
              onClick={() => handleBrandSelect(brand)}
            />
          ))}
        </CellGroup>
      )}
    </>
  );

  // Render profile list
  const renderProfileList = () => (
    <>
      {loading ? (
        <div style={{ display: 'flex', justifyContent: 'center', padding: 40 }}>
          <Loading type="spinner" />
        </div>
      ) : profiles.length === 0 ? (
        <div style={{ textAlign: 'center', padding: 40, color: '#999' }}>
          暂无车型数据
        </div>
      ) : (
        <CellGroup>
          {profiles.map((profile, index) => (
            <Cell
              key={index}
              title={profile.Name}
              label={profile.Description || undefined}
              clickable
              isLink
              onClick={() => handleProfileSelect(profile, index)}
            />
          ))}
        </CellGroup>
      )}
    </>
  );

  return (
    <div style={{ height: '100vh', display: 'flex', flexDirection: 'column', background: '#f7f8fa' }}>
      <NavBar
        title={selectedBrand ? `选择车型 - ${selectedBrand}` : '选择品牌'}
        leftText="返回"
       
        onClickLeft={handleBack}
      />

      <div style={{ flex: 1, overflow: 'auto' }}>
        {selectedBrand ? renderProfileList() : renderBrandList()}
      </div>
    </div>
  );
};

export default VehicleConfig;
