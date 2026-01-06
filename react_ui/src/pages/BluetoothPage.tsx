import React, { useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { NavBar, Form, Field, Button, Toast } from 'react-vant';
import { AppContext } from '../hooks/AppContext';

const BluetoothPage: React.FC = () => {
  const navigate = useNavigate();
  const appContext = useContext(AppContext);

  if (!appContext) {
    throw new Error('BluetoothPage must be used within AppProvider');
  }

  const { selectedDevice, setSelectedDevice } = appContext;

  const onFinish = (values: { macAddress?: string }) => {
    const inputAddress = (values.macAddress || '').trim();

    if (!inputAddress) {
      Toast.fail('请输入MAC地址');
      return;
    }

    setSelectedDevice({ name: '手动配置设备', address: inputAddress });
    Toast.success('已保存连接地址');
    navigate('/');
  };

  return (
    <div style={{ height: '100vh', display: 'flex', flexDirection: 'column', background: '#f7f8fa' }}>
      <NavBar title="配置连接地址" leftText="返回" onClickLeft={() => navigate(-1)} />

      <div style={{ flex: 1, overflow: 'auto', padding: '16px' }}>
        <Form
          onFinish={onFinish}
          footer={
            <Button block type="primary" nativeType="submit">
              确认并保存
            </Button>
          }
        >
          <Form.Item name="macAddress" label="MAC地址" initialValue={selectedDevice?.address || ''}>
            <Field placeholder="例如: AA:BB:CC:DD:EE:FF" clearable />
          </Form.Item>
        </Form>

        <div style={{ marginTop: 12, color: '#888', fontSize: 12 }}>
          请输入 OBD 适配器的物理 MAC 地址，该地址将用于后续连接。
        </div>
      </div>
    </div>
  );
};

export default BluetoothPage;
