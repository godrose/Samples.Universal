﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attest.Fake.Builders;
using Attest.Fake.LightMock;
using Attest.Fake.Setup;
using LightMock;
using Samples.Client.Data.Contracts.Dto;
using Samples.Client.Data.Contracts.Providers;

namespace Samples.Client.Data.Fake.ProviderBuilders
{            
    public class WarehouseProviderBuilder : FakeBuilderBase<IWarehouseProvider>
    {
        class WarehouseProviderProxy : ProviderProxyBase<IWarehouseProvider>, IWarehouseProvider
        {
            public WarehouseProviderProxy(IInvocationContext<IWarehouseProvider> context)
                : base(context)
            {
            }

            public Task<IEnumerable<WarehouseItemDto>> GetWarehouseItems()
            {
                return Invoke(t => t.GetWarehouseItems());
            }

            public Task<bool> DeleteWarehouseItem(Guid id)
            {
                return Invoke(t => t.DeleteWarehouseItem(id));
            }

            public Task<bool> UpdateWarehouseItem(WarehouseItemDto dto)
            {
                return Invoke(t => t.UpdateWarehouseItem(dto));
            }

            public Task CreateWarehouseItem(WarehouseItemDto dto)
            {
                return Invoke(t => t.CreateWarehouseItem(dto));
            }
        }

        private readonly List<WarehouseItemDto> _warehouseItemsStorage = new List<WarehouseItemDto>();

        private WarehouseProviderBuilder() :
            base(FakeFactoryHelper.CreateFake<IWarehouseProvider>(c => new WarehouseProviderProxy(c)))
        {
            
        }

        public static WarehouseProviderBuilder CreateBuilder()
        {
            return new WarehouseProviderBuilder();
        }

        public void WithWarehouseItems(IEnumerable<WarehouseItemDto> warehouseItems)
        {
            _warehouseItemsStorage.Clear();
            _warehouseItemsStorage.AddRange(warehouseItems);
        }

        protected override void SetupFake()
        {
            var initialSetup = ServiceCallFactory.CreateServiceCall(FakeService);

            var setup = initialSetup
                .AddMethodCallWithResultAsync(t => t.GetWarehouseItems(),
                    r => r.Complete(GetWarehouseItems))
                .AddMethodCallWithResultAsync<Guid, bool>(t => t.DeleteWarehouseItem(It.IsAny<Guid>()),
                    (r, id) => r.Complete(DeleteWarehouseItem(id)))
                .AddMethodCallWithResultAsync<WarehouseItemDto, bool>(t => t.UpdateWarehouseItem(It.IsAny<WarehouseItemDto>()),
                    (r, dto) => r.Complete(k =>
                    {
                        SaveWarehouseItem(k);
                        return true;
                    }))
                .AddMethodCallAsync<WarehouseItemDto>(t => t.CreateWarehouseItem(It.IsAny<WarehouseItemDto>()),
                    (r, dto) => r.Complete(SaveWarehouseItem));

            setup.Build();
        }

        private IEnumerable<WarehouseItemDto> GetWarehouseItems()
        {
            return _warehouseItemsStorage;
        }

        private bool DeleteWarehouseItem(Guid id)
        {
            var dto = _warehouseItemsStorage.SingleOrDefault(x => x.Id == id);
            return dto != null && _warehouseItemsStorage.Remove(dto);
        }

        private void SaveWarehouseItem(WarehouseItemDto dto)
        {
            var oldDto = _warehouseItemsStorage.SingleOrDefault(x => x.Id == dto.Id);
            if (oldDto == null)
            {
                _warehouseItemsStorage.Add(dto);
                return;
            }

            oldDto.Kind = dto.Kind;
            oldDto.Price = dto.Price;
            oldDto.Quantity = dto.Quantity;
        }
    }
}
