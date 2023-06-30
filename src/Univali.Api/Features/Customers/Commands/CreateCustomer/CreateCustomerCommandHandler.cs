using AutoMapper;
using FluentValidation;
using MediatR;
using Univali.Api.Entities;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerCommandResponse> {
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCustomerCommand> _validator;

    public CreateCustomerCommandHandler(ICustomerRepository repository, IMapper mapper, IValidator<CreateCustomerCommand> validator) {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }



    public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken) 
    {

        CreateCustomerCommandResponse createCustomerCommandResponse = new();

        var validationResult = _validator.Validate(request);

        if(!validationResult.IsValid)
        {
            foreach (var error in validationResult.ToDictionary())
            {
                createCustomerCommandResponse.Errors.Add(error.Key, error.Value);
            }

            createCustomerCommandResponse.IsSuccess = false;
            return createCustomerCommandResponse;
        }

        var newCustomer = _mapper.Map<Customer>(request.Dto);

        _repository.AddCustomer(newCustomer);
        await _repository.SaveChangesAsync();

        createCustomerCommandResponse.Customer = _mapper.Map<CreateCustomerDto>(newCustomer);

        return createCustomerCommandResponse;
    }
}