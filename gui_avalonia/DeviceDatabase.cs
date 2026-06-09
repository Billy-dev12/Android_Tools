using System;
using System.Collections.Generic;

namespace gui_avalonia
{
    public static class DeviceDatabase
    {
        public static Dictionary<string, List<DeviceModel>> Brands { get; } = new Dictionary<string, List<DeviceModel>>(StringComparer.OrdinalIgnoreCase);

        static DeviceDatabase()
        {
            // 1. Samsung
            Brands["Samsung"] = new List<DeviceModel>
            {
                new DeviceModel("Galaxy S23 Ultra", "SM-S918B"),
                new DeviceModel("Galaxy A54 5G", "SM-A546B"),
                new DeviceModel("Galaxy M34 5G", "SM-M346B"),
                new DeviceModel("Galaxy Z Fold 5", "SM-F946B"),
                new DeviceModel("Galaxy S21 FE", "SM-G990B")
            };

            // 2. Xiaomi
            Brands["Xiaomi"] = new List<DeviceModel>
            {
                new DeviceModel("Redmi Note 12 Pro", "ruby"),
                new DeviceModel("Xiaomi 13 Pro", "nuwa"),
                new DeviceModel("POCO F5", "marble"),
                new DeviceModel("Redmi 12C", "earth"),
                new DeviceModel("Xiaomi Pad 6", "pipa")
            };

            // 3. OPPO
            Brands["OPPO"] = new List<DeviceModel>
            {
                new DeviceModel("OPPO Reno 10 Pro", "CPH2525"),
                new DeviceModel("OPPO A78 5G", "CPH2495"),
                new DeviceModel("OPPO Find N2 Flip", "CPH2437"),
                new DeviceModel("OPPO A17", "CPH2477"),
                new DeviceModel("OPPO Reno 8T", "CPH2505")
            };

            // 4. Vivo
            Brands["Vivo"] = new List<DeviceModel>
            {
                new DeviceModel("Vivo V27 5G", "V2246"),
                new DeviceModel("Vivo Y36", "V2247"),
                new DeviceModel("Vivo X90 Pro", "V2219"),
                new DeviceModel("Vivo Y16", "V2204"),
                new DeviceModel("iQOO Z7 5G", "I2215")
            };

            // 5. Realme
            Brands["Realme"] = new List<DeviceModel>
            {
                new DeviceModel("Realme 11 Pro+ 5G", "RMX3741"),
                new DeviceModel("Realme C55", "RMX3710"),
                new DeviceModel("Realme GT Neo 5", "RMX3706"),
                new DeviceModel("Realme 10 Pro", "RMX3660"),
                new DeviceModel("Realme C30", "RMX3581")
            };

            // 6. Infinix
            Brands["Infinix"] = new List<DeviceModel>
            {
                new DeviceModel("Infinix Note 30 Pro", "X6716B"),
                new DeviceModel("Infinix Hot 30", "X669"),
                new DeviceModel("Infinix Zero Ultra", "X6820"),
                new DeviceModel("Infinix Smart 7", "X6515"),
                new DeviceModel("Infinix GT 10 Pro", "X6739")
            };

            // 7. Tecno
            Brands["Tecno"] = new List<DeviceModel>
            {
                new DeviceModel("Tecno Pova 5 Pro", "LH8n"),
                new DeviceModel("Tecno Spark 10 Pro", "KI7"),
                new DeviceModel("Tecno Camon 20 Pro", "CK7n"),
                new DeviceModel("Tecno Phantom V Fold", "AD10"),
                new DeviceModel("Tecno Pop 7 Pro", "BF7")
            };

            // 8. Itel
            Brands["Itel"] = new List<DeviceModel>
            {
                new DeviceModel("Itel S23", "S665L"),
                new DeviceModel("Itel P40", "P662L"),
                new DeviceModel("Itel A60s", "A662LM"),
                new DeviceModel("Itel Vision 5", "S663L"),
                new DeviceModel("Itel Spirit 2", "IT1508")
            };

            // 9. Nokia
            Brands["Nokia"] = new List<DeviceModel>
            {
                new DeviceModel("Nokia G42 5G", "TA-1581"),
                new DeviceModel("Nokia C32", "TA-1534"),
                new DeviceModel("Nokia X30 5G", "TA-1450"),
                new DeviceModel("Nokia 105 (2023)", "TA-1566"),
                new DeviceModel("Nokia G21", "TA-1418")
            };

            // 10. Motorola
            Brands["Motorola"] = new List<DeviceModel>
            {
                new DeviceModel("Moto G84 5G", "XT2347"),
                new DeviceModel("Edge 40 Neo", "XT2307"),
                new DeviceModel("Razr 40 Ultra", "XT2321"),
                new DeviceModel("Moto E13", "XT2345"),
                new DeviceModel("Moto G54", "XT2343")
            };

            // 11. OnePlus
            Brands["OnePlus"] = new List<DeviceModel>
            {
                new DeviceModel("OnePlus 11 5G", "CPH2449"),
                new DeviceModel("OnePlus Nord 3", "CPH2493"),
                new DeviceModel("OnePlus Nord CE 3 Lite", "CPH2467"),
                new DeviceModel("OnePlus 11R", "CPH2487"),
                new DeviceModel("OnePlus Ace 2", "PHP110")
            };

            // 12. Huawei
            Brands["Huawei"] = new List<DeviceModel>
            {
                new DeviceModel("Huawei P60 Pro", "MNA-AL00"),
                new DeviceModel("Huawei Mate 60 Pro", "ALN-AL00"),
                new DeviceModel("Huawei Nova 11 Pro", "GOA-AL00"),
                new DeviceModel("Huawei Mate X3", "ALT-AL00"),
                new DeviceModel("Huawei Y90", "CTR-LX1")
            };
        }
    }
}
