﻿using IVCFB2_HSZF_2024251.Model;

namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public interface ICarDataProvider
    {
        IEnumerable<Car> GetAllCars();
        Car GetCarById(int id);

        void CarsToExcel();
        void AddCar(Car car);
        void UpdateCar(Car car);
        void DeleteCar(Car car);

        //void DeleteCars();
        void DeleteAllCar();
    }
}
