import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../shared/Models/pagination';
import { Product } from '../shared/Models/product';
import { Brand } from '../shared/Models/type';
import { Type } from '../shared/Models/brand';
import { ShopParams } from '../shared/Models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseURL='https://localhost:5001/api/';

  constructor(private http : HttpClient) { }



  getProducts(shopParams:ShopParams){
    let params = new HttpParams();

    if (shopParams.brandId) params= params.append('brandId', shopParams.brandId);
    if (shopParams.typeId) params= params.append('typeId', shopParams.typeId);
    params= params.append('sort', shopParams.sort);
    params= params.append('pageIndex', shopParams.pageNumber);
    params= params.append('pageSize', shopParams.pageSize);
    if(shopParams.search) params= params.append('search', shopParams.search);


  
    return this.http.get<Pagination<Product[]>>(this.baseURL+'products', {params});
  }
  getProduct(id: number){
    return this.http.get<Product>(this.baseURL + 'products/' + id);
  }
  getBrands(){
    return this.http.get<Brand[]>(this.baseURL+'products/brands');
  }
  getTypes(){
    return this.http.get<Type[]>(this.baseURL+'products/types');
  }
}
