export interface CustomerDto {
  id: number
  name: string
  abbreviation?: string
}

export interface ProductDto {
  id: number
  name: string
  description?: string
}

export interface SystemDto {
  id: number
  customerId: number
  customerName: string
  productId: number
  productName: string
  pocName?: string
  pocEmail?: string
  pocPhone?: string
  parentId?: number
}

export interface SystemVersionDto {
  id: number
  systemId: number
  versionNumber: string
  svdPath?: string
  svmPath?: string
  status: string
  releasedDate?: Date
  pocName?: string
  pocEmail?: string
  pocPhone?: string
  parentId?: number
}

export interface SegmentDto {
  id: number
  systemVersionId: number
  name: string
  versionNumber: string
  status: string
  releasedDate?: Date
  pocName?: string
  pocEmail?: string
  pocPhone?: string
  parentId?: number
}

export interface CsciDto {
  id: number
  name: string
  versionNumber: string
  status: string
  releasedDate?: Date
  releaseLocation?: string
  chargeNumber?: string
  pocName?: string
  pocEmail?: string
  pocPhone?: string
  parentId?: number
}
